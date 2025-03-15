using AutoMapper;
using Azure.Core;
using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using TaskManagerWebApi.AuthorizationPolicy;
using TaskManagerWebApi.Controllers.ControllerHelper;
using TaskManagerWebApi.DTO_s.TaskDTO_s;
using TaskManagerWebApi.Models;
using TaskManagerWebApi.Service.Interfaces;
using TaskManagerWebApi.Models.NewFolder;

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

        public async Task<IActionResult> GetAll([FromQuery] TaskGetAllRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await taskService.GetAllTasks(request);

            var tasksGet = result.Items.Select(task => mapper.Map<TaskGetRequest>(task)).ToList();

            var response = new PagedResult<TaskGetRequest>(
            result.TotalCount,
            request.Offset ?? 0,
                request.Limit ?? 10,
                tasksGet
            );

            return Ok(response);
            
        }
          
        
        [HttpPost]
        public async Task<IActionResult> CreateTask([FromBody][Required] TaskAddRequest taskaddRequest)
        {
            var addedTask = mapper.Map<UserTask>(taskaddRequest);
            addedTask.IdUser = taskaddRequest.UserId;
            var task = await taskService.CreateTask(addedTask, new User());
            if (task.IsFailed)
            {
                return ProcessError(task.Errors);
            }
            return Ok(task.Value);
        }

        [HttpPut]
        [Authorize(AuthorizationPoilicyConstants.MANAGER_POLICY)]
        public async Task<IActionResult> UpdateTaskForManager([FromBody][Required] TaskUpdateRequest taskUpdateRequest)
        {
            var updatedTaskMapped = mapper.Map<UserTask>(taskUpdateRequest);
            //updatedTaskMapped.IdUser = taskUpdateRequest.UserId;
            var task = await taskService.UpdateTask(updatedTaskMapped);

            if (task.IsFailed)
            {
                return ProcessError(task.Errors);
            }
            return Ok(task.Value);

        }
        [HttpPatch("{taskId}")]

        public async Task<IActionResult> UpdateTaskStatus(int taskId, [FromBody] UpdateTaskStatusRequest request)
        {
            if (!Enum.TryParse<TaskStatuses>(request.Status, true, out var statusEnum))
            {
                return BadRequest("Invalid task status.");
            }

            var result = await taskService.UpdateTaskStatus(taskId, statusEnum, request.RejectionReason);

            if (result.IsSuccess)
                return Ok(new { message = "Status has been updated successfully." });

            return BadRequest(result.Errors);
        }

        [Authorize(AuthorizationPoilicyConstants.MANAGER_POLICY)]
        [HttpPatch("{taskId}")]
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
            if (task.IsFailed)
            {
                return ProcessError(task.Errors);
            }
            return Ok(new { message = "Task has been deleted successfully." });
        }


    }
}

