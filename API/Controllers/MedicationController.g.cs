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
    /// Controller responsible for managing medication related operations.
    /// </summary>
    /// <remarks>
    /// This Controller provides endpoints for adding, retrieving, updating, and deleting medication information.
    /// </remarks>
    [Route("api/medication")]
    [Authorize]
    public class MedicationController : ControllerBase
    {
        private readonly IMedicationService _medicationService;

        /// <summary>
        /// Initializes a new instance of the MedicationController class with the specified context.
        /// </summary>
        /// <param name="imedicationservice">The imedicationservice to be used by the controller.</param>
        public MedicationController(IMedicationService imedicationservice)
        {
            _medicationService = imedicationservice;
        }

        /// <summary>Adds a new medication</summary>
        /// <param name="model">The medication data to be added</param>
        /// <returns>The result of the operation</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces("application/json")]
        [UserAuthorize("Medication",Entitlements.Create)]
        public IActionResult Post([FromBody] Medication model)
        {
            var id = _medicationService.Create(model);
            return Ok(new { id });
        }

        /// <summary>Retrieves a list of medications based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of medications</returns>
        [HttpGet]
        [UserAuthorize("Medication",Entitlements.Read)]
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

            var result = _medicationService.Get(filterCriteria, searchTerm, pageNumber, pageSize, sortField, sortOrder);
            return Ok(result);
        }

        /// <summary>Retrieves a specific medication by its primary key</summary>
        /// <param name="id">The primary key of the medication</param>
        /// <returns>The medication data</returns>
        [HttpGet]
        [Route("{id:Guid}")]
        [UserAuthorize("Medication",Entitlements.Read)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces("application/json")]
        public IActionResult GetById([FromRoute] Guid id)
        {
            var result = _medicationService.GetById(id);
            return Ok(result);
        }

        /// <summary>Deletes a specific medication by its primary key</summary>
        /// <param name="id">The primary key of the medication</param>
        /// <returns>The result of the operation</returns>
        [HttpDelete]
        [UserAuthorize("Medication",Entitlements.Delete)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Produces("application/json")]
        [Route("{id:Guid}")]
        public IActionResult DeleteById([FromRoute] Guid id)
        {
            var status = _medicationService.Delete(id);
            return Ok(new { status });
        }

        /// <summary>Updates a specific medication by its primary key</summary>
        /// <param name="id">The primary key of the medication</param>
        /// <param name="updatedEntity">The medication data to be updated</param>
        /// <returns>The result of the operation</returns>
        [HttpPut]
        [UserAuthorize("Medication",Entitlements.Update)]
        [Route("{id:Guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        public IActionResult UpdateById(Guid id, [FromBody] Medication updatedEntity)
        {
            if (id != updatedEntity.Id)
            {
                return BadRequest("Mismatched Id");
            }

            var status = _medicationService.Update(id, updatedEntity);
            return Ok(new { status });
        }

        /// <summary>Updates a specific medication by its primary key</summary>
        /// <param name="id">The primary key of the medication</param>
        /// <param name="updatedEntity">The medication data to be updated</param>
        /// <returns>The result of the operation</returns>
        [HttpPatch]
        [UserAuthorize("Medication",Entitlements.Update)]
        [Route("{id:Guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        public IActionResult UpdateById(Guid id, [FromBody] JsonPatchDocument<Medication> updatedEntity)
        {
            if (updatedEntity == null)
                return BadRequest("Patch document is missing.");
            var status = _medicationService.Patch(id, updatedEntity);
            return Ok(new { status });
        }
    }
}