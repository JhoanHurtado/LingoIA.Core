using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using LingoIA.Application.Dtos;
using LingoIA.Domain.Entities;

namespace LingoIA.Application.Mappings
{
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<User, UserDto>().ReverseMap();
        CreateMap<RegisterUserDto, User>().ReverseMap();
        CreateMap<LoginUserDto, User>().ReverseMap();
        CreateMap<Conversation, ConversationDto>().ReverseMap();
        CreateMap<Message, MessageDto>().ReverseMap();
    }
}
}