using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using StudySync.DTOs;
using StudySync.Models;
using StudySync.Repositories;
using System.Threading.Tasks;

namespace StudySync.Services
{
    public class SubtaskService : ISubtaskService
    {
        private readonly ISubtaskRepository _subtaskRepository;
        private readonly IMapper _mapper;
        public SubtaskService(ISubtaskRepository subtaskRepository,IMapper mapper)
        {
            _subtaskRepository = subtaskRepository;
            _mapper = mapper;

        }

        public async Task<IEnumerable<SubtaskDTO>> GetAllSubtasksAsync()
        {
            var subtasks= await _subtaskRepository.GetAllSubtasksAsync();
            return _mapper.Map<IEnumerable<SubtaskDTO>>(subtasks);
        }

        public async Task<IEnumerable<SubtaskDTO>> GetSubtasksbyTaskIdAsync(int taskId)
        {
            var subtask = await _subtaskRepository.GetSubtasksbyTaskIdAsync(taskId);
            return _mapper.Map<IEnumerable<SubtaskDTO>>(subtask);
        }

        public async Task<SubtaskDTO> GetSubtaskByIdAsync(int id)
        {
            var subtask = await _subtaskRepository.GetSubtaskByIdAsync(id);
            return _mapper.Map<SubtaskDTO>(subtask);
        }

        public async Task<SubtaskDTO> AddSubtaskAsync(SubtaskCreateDTO createDto)
        {
            var subtask = _mapper.Map<Subtask>(createDto);
            await _subtaskRepository.AddSubtaskAsync(subtask);
            return _mapper.Map<SubtaskDTO>(subtask);
        }

        public async Task UpdateSubtaskAsync(int id, SubtaskUpdateDTO updateDto)
        {
            var subtask = await _subtaskRepository.GetSubtaskByIdAsync(id);
            if(subtask == null)
            {
                throw new KeyNotFoundException($"Subtask  not found.");
            }

            _mapper.Map(updateDto, subtask);
            await _subtaskRepository.UpdateSubtaskAsync(subtask);
        }

        public async Task DeleteSubtaskAsync(int id)
        {
            await _subtaskRepository.DeleteSubtaskAsync(id);
            
        }


        public async Task<IEnumerable<SubtaskDTO>> GetIncompleteSubtasksByTaskIdAsync(int taskId)
        {
            var subtasks = await _subtaskRepository.GetIncompleteSubtasksByTaskIdAsync(taskId);
            return _mapper.Map<IEnumerable<SubtaskDTO>>(subtasks);
        }

        
    }
}
