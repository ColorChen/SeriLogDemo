using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SeriLogDemo_Net6.Model;
using SeriLogDemo_Net6.Repository;

namespace WebApplication2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkForDapperController : ControllerBase
    {
        /// <summary>
        /// 卡片資料操作
        /// </summary>
        private readonly WorkRepository _workRepository;

        public WorkForDapperController(WorkRepository workRepository)
        {
            _workRepository = workRepository;
        }
        /// <summary>
        /// 查詢卡片列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<Work> GetList()
        {
            return this._workRepository.GetList();
        }

        /// <summary>
        /// 查詢卡片
        /// </summary>
        /// <param name="id">卡片編號</param>
        /// <returns></returns>
        [HttpGet]
        [Route("{id}")]
        public Work Get([FromRoute] int id)
        {
            var result = this._workRepository.Get(id);
            if (result is null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return result;
        }

        /// <summary>
        /// 新增卡片
        /// </summary>
        /// <param name="parameter">卡片參數</param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Insert([FromBody] WorkParameter parameter)
        {
            var result = this._workRepository.Create(parameter);
            if (result > 0)
            {
                return Ok();
            }
            return StatusCode(500);
        }

        /// <summary>
        /// 更新卡片
        /// </summary>
        /// <param name="id">卡片編號</param>
        /// <param name="parameter">卡片參數</param>
        /// <returns></returns>
        [HttpPut]
        [Route("{id}")]
        public IActionResult Update(
            [FromRoute] int id,
            [FromBody] WorkParameter parameter)
        {
            var targetWork = this._workRepository.Get(id);
            if (targetWork is null)
            {
                return NotFound();
            }

            var isUpdateSuccess = this._workRepository.Update(id, parameter);
            if (isUpdateSuccess)
            {
                return Ok();
            }
            return StatusCode(500);
        }

        /// <summary>
        /// 刪除卡片
        /// </summary>
        /// <param name="id">卡片編號</param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{id}")]
        public IActionResult Delete([FromRoute] int id)
        {
            this._workRepository.Delete(id);
            return Ok();
        }
    }
}
