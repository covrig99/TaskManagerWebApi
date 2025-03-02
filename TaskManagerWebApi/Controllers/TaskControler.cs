using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TaskManagerWebApi.DTO_s.TaskDTO_s;
using TaskManagerWebApi.Models;
using TaskManagerWebApi.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using TaskManagerWebApi.Controllers.ControllerHelper;

namespace TaskManagerWebApi.Controllers
{
    [Route("[controller]/[Action]")]
    public class TaskControler : ControllerResponseHelper

    {
        private readonly ITaskService taskService;
        private readonly IMapper mapper;

        public TaskControler(ITaskService service,IMapper mapper)

        {
            taskService = service;
            this.mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await taskService.GetAllTasks();
            return Ok(result);
        }
        [HttpPost]
        public async Task<IActionResult> CreateTask([FromBody][Required] TaskAddRequest taskaddRequest)
        {
            var addedTask = mapper.Map<UserTask>(taskaddRequest);
            User user = new User();
            var task = await taskService.CreateTask(addedTask, user);
            if(task.IsSuccess)
            {
                return Ok(task.Value);
            }
            else
            {
                return ProcessError(task.Errors);
            }
        }
        [HttpPut]
        public async Task<IActionResult> UpdateTask([FromBody][Required] TaskUpdateRequest taskUpdateRequest)
        {
            var updatedTaskMapped = mapper.Map<UserTask>(taskUpdateRequest);
            User user = new User();
            var task = await taskService.UpdateTask(updatedTaskMapped);
            
            if (task.IsSuccess)
            {
                return Ok(task.Value);
            }
            else
            {
                return ProcessError(task.Errors);
            }

        }
        [HttpDelete]
        public async Task<IActionResult> DeleteTask(int taskId)
        {
            var task = await taskService.DeleteTask(taskId);
            if (task.IsSuccess)
            {
                return Ok(task.Value);
            }
            else
            {
                return ProcessError(task.Errors);
            }
        }


    }   
}

