using Microsoft.AspNetCore.Mvc;
using Nov12.Models;
using Nov12.Services;
using Nov12.Entities;
using Nov12.Filter;
using Nov12.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;

namespace Nov12.Controllers
{
    /// <summary>
    /// Controller responsible for managing visitvitaltemplateparameter related operations.
    /// </summary>
    /// <remarks>
    /// This Controller provides endpoints for adding, retrieving, updating, and deleting visitvitaltemplateparameter information.
    /// </remarks>
    [Route("api/visitvitaltemplateparameter")]
    [Authorize]
    public class VisitVitalTemplateParameterController : ControllerBase
    {
        private readonly IVisitVitalTemplateParameterService _visitVitalTemplateParameterService;

        /// <summary>
        /// Initializes a new instance of the VisitVitalTemplateParameterController class with the specified context.
        /// </summary>
        /// <param name="ivisitvitaltemplateparameterservice">The ivisitvitaltemplateparameterservice to be used by the controller.</param>
        public VisitVitalTemplateParameterController(IVisitVitalTemplateParameterService ivisitvitaltemplateparameterservice)
        {
            _visitVitalTemplateParameterService = ivisitvitaltemplateparameterservice;
        }

        /// <summary>Adds a new visitvitaltemplateparameter</summary>
        /// <param name="model">The visitvitaltemplateparameter data to be added</param>
        /// <returns>The result of the operation</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces("application/json")]
        [UserAuthorize("VisitVitalTemplateParameter",Entitlements.Create)]
        public IActionResult Post([FromBody] VisitVitalTemplateParameter model)
        {
            var id = _visitVitalTemplateParameterService.Create(model);
            return Ok(new { id });
        }

        /// <summary>Retrieves a list of visitvitaltemplateparameters based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of visitvitaltemplateparameters</returns>
        [HttpGet]
        [UserAuthorize("VisitVitalTemplateParameter",Entitlements.Read)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        public IActionResult Get([FromQuery] string filters, string searchTerm, int pageNumber = 1, int pageSize = 10, string sortField = null, string sortOrder = "asc")
        {
            List<FilterCriteria> filterCriteria = null;
            if (pageSize < 1)
            {
                return BadRequest("Page size invalid.");
            }

            if (pageNumber < 1)
            {
                return BadRequest("Page mumber invalid.");
            }

            if (!string.IsNullOrEmpty(filters))
            {
                filterCriteria = JsonHelper.Deserialize<List<FilterCriteria>>(filters);
            }

            var result = _visitVitalTemplateParameterService.Get(filterCriteria, searchTerm, pageNumber, pageSize, sortField, sortOrder);
            return Ok(result);
        }

        /// <summary>Retrieves a specific visitvitaltemplateparameter by its primary key</summary>
        /// <param name="id">The primary key of the visitvitaltemplateparameter</param>
        /// <returns>The visitvitaltemplateparameter data</returns>
        [HttpGet]
        [Route("{id:Guid}")]
        [UserAuthorize("VisitVitalTemplateParameter",Entitlements.Read)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces("application/json")]
        public IActionResult GetById([FromRoute] Guid id)
        {
            var result = _visitVitalTemplateParameterService.GetById(id);
            return Ok(result);
        }

        /// <summary>Deletes a specific visitvitaltemplateparameter by its primary key</summary>
        /// <param name="id">The primary key of the visitvitaltemplateparameter</param>
        /// <returns>The result of the operation</returns>
        [HttpDelete]
        [UserAuthorize("VisitVitalTemplateParameter",Entitlements.Delete)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Produces("application/json")]
        [Route("{id:Guid}")]
        public IActionResult DeleteById([FromRoute] Guid id)
        {
            var status = _visitVitalTemplateParameterService.Delete(id);
            return Ok(new { status });
        }

        /// <summary>Updates a specific visitvitaltemplateparameter by its primary key</summary>
        /// <param name="id">The primary key of the visitvitaltemplateparameter</param>
        /// <param name="updatedEntity">The visitvitaltemplateparameter data to be updated</param>
        /// <returns>The result of the operation</returns>
        [HttpPut]
        [UserAuthorize("VisitVitalTemplateParameter",Entitlements.Update)]
        [Route("{id:Guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        public IActionResult UpdateById(Guid id, [FromBody] VisitVitalTemplateParameter updatedEntity)
        {
            if (id != updatedEntity.Id)
            {
                return BadRequest("Mismatched Id");
            }

            var status = _visitVitalTemplateParameterService.Update(id, updatedEntity);
            return Ok(new { status });
        }

        /// <summary>Updates a specific visitvitaltemplateparameter by its primary key</summary>
        /// <param name="id">The primary key of the visitvitaltemplateparameter</param>
        /// <param name="updatedEntity">The visitvitaltemplateparameter data to be updated</param>
        /// <returns>The result of the operation</returns>
        [HttpPatch]
        [UserAuthorize("VisitVitalTemplateParameter",Entitlements.Update)]
        [Route("{id:Guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        public IActionResult UpdateById(Guid id, [FromBody] JsonPatchDocument<VisitVitalTemplateParameter> updatedEntity)
        {
            if (updatedEntity == null)
                return BadRequest("Patch document is missing.");
            var status = _visitVitalTemplateParameterService.Patch(id, updatedEntity);
            return Ok(new { status });
        }
    }
}