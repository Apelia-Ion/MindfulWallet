using Microsoft.AspNetCore.Mvc;
using MindfulWallet.Aplication.Interfaces.Service;
using MindfulWallet.Core.Entities;
using MindfulWallet.Core.DTOs;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MindfulWalletAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GoalController : ControllerBase
    {
        private readonly IGoalService _goalService;

        public GoalController(IGoalService goalService)
        {
            _goalService = goalService;
        }

        [HttpPost]
        public async Task<IActionResult> AddGoal([FromBody] GoalDto goalDto)
        {
            var goal = new Goal
            {
                UserId = goalDto.UserId,
                Title = goalDto.Title,
                Description = goalDto.Description,
                Motivation = goalDto.Motivation,
                DueDate = goalDto.DueDate,
                Amount = goalDto.Amount,
                Status = goalDto.Status
                
            };

            var createdGoal = await _goalService.AddGoalAsync(goal);
            return CreatedAtAction(nameof(GetGoalsByUserId), new { userId = createdGoal.UserId }, createdGoal);
        }

        [HttpDelete("{goalId}")]
        public async Task<IActionResult> DeleteGoal(int goalId)
        {
            var result = await _goalService.DeleteGoalAsync(goalId);
            if (!result)
            {
                return NotFound(new { Message = "Goal not found" });
            }
            return NoContent();
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetGoalsByUserId(int userId)
        {
            var goals = await _goalService.GetGoalsByUserIdAsync(userId);
            var goalDtos = goals.Select(goal => new GoalDto
            {
                Id = goal.Id,
                UserId = goal.UserId,
                Title = goal.Title,
                Description = goal.Description,
                Motivation = goal.Motivation,
                DueDate = goal.DueDate,
                Amount = goal.Amount,
                Status = goal.Status
            });

            return Ok(goalDtos);
        }
    }
}
