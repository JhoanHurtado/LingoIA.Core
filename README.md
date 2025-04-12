# 🧠 LingoIA - Asistente de Voz Multilingüe

**LingoIA** es una aplicación de conversación impulsada por inteligencia artificial que permite a los usuarios **practicar idiomas de manera natural, fluida y personalizada**. Utiliza reconocimiento de voz, procesamiento de lenguaje natural y síntesis de voz para ofrecer una experiencia conversacional completa.

---

## 🚀 Funcionalidades Principales

### 🎙️ Interacción por Voz

- Conversaciones completamente por voz (solo registro/login requiere interfaz táctil).
- Reconocimiento de voz mediante **Whisper (STT)**.
- Respuestas habladas con **Coqui TTS** para una voz natural y realista.

---

### 🌍 Soporte Multilingüe

- Elección del idioma de conversación (español, inglés, entre otros).
- Cambio de idioma de las respuestas en tiempo real.
- Respuestas adaptadas al nivel del usuario (básico, intermedio, avanzado).

---

### 📚 Aprendizaje Adaptativo

- Corrección gramatical y sugerencias de mejora durante la conversación.
- Evaluación automática de las respuestas con puntuación del 0 al 100.

---

### 🧠 Inteligencia Conversacional

- Preguntas guiadas por tema elegido por el usuario (viajes, tecnología, comida, etc).
- Comprensión y generación de lenguaje natural.
- Capacidad para responder a preguntas del usuario sobre el idioma o el tema en conversación.

---

## 🧩 Arquitectura

### 🖥️ Backend (.NET 8)

- API REST desarrollada en **.NET 8** con arquitectura modular y escalable.
- Integración con:
  - **Whisper** para transcripción de voz.
  - **Coqui TTS** para síntesis de texto a voz.
  - **PostgreSQL** como base de datos principal.

---

### 📱 Frontend (Flutter)

- Aplicación móvil multiplataforma (Android & iOS).
- Flujo de usuario simple: iniciar sesión, elegir idioma y comenzar a hablar.
- Grabación de voz y reproducción de respuestas habladas.
- Configuración de idioma, nivel de dificultad y temas preferidos.

---


## 🔧 Tecnologías Clave

| Componente       | Tecnología                    |
|------------------|-------------------------------|
| Backend API      | .NET 8                        |
| Frontend App     | Flutter                       |
| STT              | Whisper                       |
| TTS              | Coqui TTS                     |
| DB               | PostgreSQL                    |

---

## 🧪 Estado del Proyecto

> Este es un **Demo** en desarrollo. Las funciones básicas están implementadas y se están realizando pruebas para mejorar la experiencia conversacional, optimizar el rendimiento y ampliar los idiomas disponibles.
