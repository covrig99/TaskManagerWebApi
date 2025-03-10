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
using System.ComponentModel.DataAnnotations;
using TaskManagerWebApi.AuthorizationPolicy;

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
            if(task.IsSuccess)
            {
                return Ok(task.Value);
            }
            else
            {
                return ProcessError(task.Errors);
            }
        }
        [Authorize(AuthorizationPoilicyConstants.MANAGER_POLICY)]
        [HttpPut]
        public async Task<IActionResult> AssigneTaskbyManager([FromBody][Required] TaskUpdataRequest taskUpdateRequest)
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
        public async Task<IActionResult> UpdateTaskStatus(int taskId, [FromBody] UpdateTaskStatusRequest request)
        {
            var result = await taskService.UpdateTaskStatus(taskId, request.Status, request.RejectionReason, request.UserId);

            if (result.IsSuccess)
                return Ok(result.Value);

            return BadRequest(result.Errors);
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

