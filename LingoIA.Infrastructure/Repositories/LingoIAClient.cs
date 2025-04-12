using LingoIA.Application.Interfaces;
using LingoIA.Domain.models;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace LingoIA.Infrastructure.Repositories
{
    public class LingoIAClient : ILingoIAClient
    {
        private readonly HttpClient _httpClient;
        private readonly string _assistantName = "LingoIA";
        private readonly string _apiUrl = "https://openrouter.ai/api/v1/chat/completions";
        private readonly List<Dictionary<string, string>> _messageHistory;

        private readonly IConfiguration _configuration;

        private string _language = "english";
        private string _level = "intermediate";
        private MessageAnalysis? _lastAnalysis;

        public LingoIAClient(IConfiguration configuration)
        {
            _configuration = configuration;
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _configuration.GetSection("settingKey")["APIKey"]);
            _httpClient.DefaultRequestHeaders.Add("HTTP-Referer", "https://github.com/LingoIA-App");
            _httpClient.DefaultRequestHeaders.Add("X-Title", "LingoIA Language Assistant");

            _messageHistory = new List<Dictionary<string, string>>();
        }

        public async Task<string> StartNewConversation(string language, string topic, string username)
        {
            _messageHistory.Clear();
            _language = language.ToLower();
            var systemPrompt = BuildSystemPrompt(language, _level, username);
            _messageHistory.Add(new Dictionary<string, string>
            {
                { "role", "system" },
                { "content", systemPrompt }
            });

            var prompt = _language == "spanish"
                ? $"Inicia un diálogo sobre {topic} en español. Usa mi nombre ({username}) 2-3 veces naturalmente. Hazme 1 pregunta para continuar."
                : $"Start a {topic} conversation in English. Use my name ({username}) 2-3 times naturally. Ask me 1 follow-up question.";

            return await SendMessage(prompt, _messageHistory, username);
        }

        public async Task<string> SendMessage(string message, List<Dictionary<string, string>> historyMessage, string username)
        {
            _messageHistory.Add(new Dictionary<string, string>
            {
                { "role", "user" },
                { "content", $"[Usuario: {username}] {message}" }
            });

            // Analizar automáticamente el mensaje del usuario
            _lastAnalysis = await AnalyzeMessage(message);

            var requestData = new
            {
                model = "google/gemini-2.5-pro-exp-03-25:free",
                messages = _messageHistory,
                temperature = 0.7
            };

            var jsonContent = JsonSerializer.Serialize(requestData);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(_apiUrl, content);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var jsonResponse = JsonDocument.Parse(responseContent);
                var reply = jsonResponse.RootElement
                    .GetProperty("choices")[0]
                    .GetProperty("message")
                    .GetProperty("content")
                    .GetString();

                _messageHistory.Add(new Dictionary<string, string>
                {
                    { "role", "assistant" },
                    { "content", reply }
                });

                return reply;
            }

            throw new Exception($"API Error: {response.StatusCode} - {await response.Content.ReadAsStringAsync()}");
        }

        public void ResetConversation()
        {
            _messageHistory.Clear();
            _lastAnalysis = null;
        }

        public MessageAnalysis? GetLastAnalysis() => _lastAnalysis;
        public List<Dictionary<string, string>> GetMessageHistory() => _messageHistory;

        private string BuildSystemPrompt(string language, string proficiencyLevel, string username)
        {
            return language.ToLower() switch
            {
                "spanish" => $"""
                Eres {_assistantName}, un asistente de idiomas. El usuario se llama {username}.
                Ayuda a {username} a practicar español (nivel {proficiencyLevel}).

                Reglas:
                1. Usa el nombre {username} naturalmente en la conversación
                2. Corrige errores señalándolos amablemente
                3. Explica correcciones con ejemplos
                4. Mantén un tono cálido y motivador
                5. Firma siempre como "- {_assistantName}"
                """,
                _ => $"""
                You are {_assistantName}, an English tutor. The user's name is {username}.
                Help {username} practice {proficiencyLevel} level English.

                Rules:
                1. Use {username}'s name naturally in conversation
                2. Correct mistakes with gentle explanations
                3. Provide clear examples
                4. Keep a warm, encouraging tone
                5. Always sign as "- {_assistantName}"
                """
            };
        }

        private string BuildAnalysisPrompt(string userText)
        {
            return _language switch
            {
                "spanish" => $$"""
                Analiza el siguiente mensaje escrito por un estudiante de español:

                "{{userText}}"

                Evalúa lo siguiente:
                - Número de errores gramaticales u ortográficos
                - Versión corregida del texto
                - Explicación de los errores
                - Puntuación general del 0.0 al 100.0 según claridad, corrección y nivel de dificultad

                Devuélvelo en formato JSON:
                {
                    "original": "...",
                    "corrected": "...",
                    "explanation": "...",
                    "score": 0.0
                }
                """,
                _ => $$"""
                Analyze the following sentence written by a student learning English:

                "{{userText}}"

                Evaluate the following:
                - Number of grammar or spelling mistakes
                - Corrected version of the sentence
                - Explanation of the mistakes
                - Overall score from 0.0 to 100.0 (clarity, accuracy, difficulty)

                Return it in JSON format:
                {
                    "original": "...",
                    "corrected": "...",
                    "explanation": "...",
                    "score": 0.0
                }
                """
            };
        }

        private async Task<MessageAnalysis?> AnalyzeMessage(string userText)
        {
            var analysisPrompt = BuildAnalysisPrompt(userText);

            var requestData = new
            {
                model = "google/gemini-2.5-pro-exp-03-25:free",
                messages = new List<Dictionary<string, string>>
                {
                    new Dictionary<string, string>
                    {
                        { "role", "system" },
                        { "content", "Eres un corrector experto de idiomas que responde en formato JSON." }
                    },
                    new Dictionary<string, string>
                    {
                        { "role", "user" },
                        { "content", analysisPrompt }
                    }
                },
                temperature = 0.2
            };

            var jsonContent = JsonSerializer.Serialize(requestData);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(_apiUrl, content);

            if (!response.IsSuccessStatusCode)
                return null;

            var responseContent = await response.Content.ReadAsStringAsync();

            try
            {
                var jsonResponse = JsonDocument.Parse(responseContent);
                var contentString = jsonResponse.RootElement
                    .GetProperty("choices")[0]
                    .GetProperty("message")
                    .GetProperty("content")
                    .GetString();

                if (string.IsNullOrWhiteSpace(contentString))
                    return null;
                contentString= contentString.Trim('`');
                if (contentString.StartsWith("json", StringComparison.OrdinalIgnoreCase))
                {
                    contentString = contentString.Substring("json".Length);
                }

                var analysis = JsonSerializer.Deserialize<MessageAnalysis>(contentString, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
                return analysis;
            }
            catch
            {
                return null;
            }
        }
    }
}
