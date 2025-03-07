using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using TaskManagerWebApi.AuthorizationPolicy;
using TaskManagerWebApi.Controllers.ControllerHelper;
using TaskManagerWebApi.DTO_s.TaskDTO_s;
using TaskManagerWebApi.Models;
using TaskManagerWebApi.Service.Interfaces;

namespace TaskManagerWebApi.Controllers
{
    [Route("[controller]/[Action]")]
    public class TaskControler : ControllerResponseHelper

    {
        private readonly ITaskService taskService;
        private readonly IMapper mapper;

        public TaskControler(ITaskService service, IMapper mapper)

        {
            taskService = service;
            this.mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await taskService.GetAllTasks();
            var tasksGetRequest = result.Select(async car =>
            {
                var taskGet = mapper.Map<TaskGetRequest>(car);
                return taskGet;
            });
            var carsGet = await Task.WhenAll(tasksGetRequest);
            return Ok(result);
        }
        [HttpPost]
        public async Task<IActionResult> CreateTask([FromBody][Required] TaskAddRequest taskaddRequest)
        {
            var addedTask = mapper.Map<UserTask>(taskaddRequest);
            addedTask.IdUser = taskaddRequest.UserId;
            var task = await taskService.CreateTask(addedTask, new User());
            if (task.IsSuccess)
            {
                return Ok(task.Value);
            }
            else
            {
                return ProcessError(task.Errors);
            }
        }
        
        [HttpPut]
        
        public async Task<IActionResult> UpdateTask([FromBody][Required] TaskUpdataRequest taskUpdateRequest)
        {
            var updatedTaskMapped = mapper.Map<UserTask>(taskUpdateRequest);
            updatedTaskMapped.IdUser = taskUpdateRequest.UserId;
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
        [HttpPatch("{taskId}")]
        [Authorize(AuthorizationPoilicyConstants.MANAGER_POLICY)]
        public async Task<IActionResult> AssigneTaskToUser([FromRoute] int taskId, [FromBody] AssignTaskByManagerRequest assigneTaskByManager)
        {
            var updatedTaskMapped = mapper.Map<UserTask>(assigneTaskByManager);
            updatedTaskMapped.IdUser = assigneTaskByManager.UserId;
            var task = await taskService.AssignTaskToUser(taskId, updatedTaskMapped.IdUser);

            if (task.IsFailed)
            {
                return ProcessError(task.Errors);
            }
            return Ok(task.Value);
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

