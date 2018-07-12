using AutoMapper;
using Services;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Workshop_TecomNetways.DTO;
using Workshop_TecomNetways.Models;
using Workshop_TecomNetways.Repository;

namespace Workshop_TecomNetways.Controllers
{

    public class SubCriteriaController : ApiController
    {
        private UnitOfWork UoW = null;
        private IMapper EntityToDtoIMapper = null;
        private IMapper DtoToEntityIMapper = null;
        

        public SubCriteriaController()
        {
            UoW = new UnitOfWork();
            InitializeMapping();

        }

        public SubCriteriaController(UnitOfWork uoW)
        {
            UoW = uoW;
            InitializeMapping();
        }

        private void InitializeMapping()
        {
            EntityToDtoIMapper = new MapperConfiguration
                (cfg => cfg.CreateMap<SubCriteria, SubCriteriaDto>())
                .CreateMapper();
            DtoToEntityIMapper = new MapperConfiguration
                (cfg => cfg.CreateMap<SubCriteriaDto, SubCriteria>())
                .CreateMapper();
        }

        // GET: api/SubCriterias
        [HttpGet]
        public IHttpActionResult GetAll()
        {
            var items = UoW.GetRepository<SubCriteria>().GetAll().ToList();
            var DTO = EntityToDtoIMapper.Map<List<SubCriteria>, List<SubCriteriaDto>>(items).ToList();

            return Ok(DTO);
        }

        // GET: api/SubCriterias/5
        [ResponseType(typeof(SubCriteria))]
        public async Task<IHttpActionResult> GetSubCriteria(int id)
        {
            SubCriteria item = await UoW.GetRepository<SubCriteria>().GetItemAsycn(e => e.ID == id);
            if (item == null)
            {
                return NotFound();
            }

            var DTO = EntityToDtoIMapper.Map<SubCriteria, SubCriteriaDto>(item);
            return Ok(DTO);
        }

        // PUT: api/SubCriterias/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutSubCriteria(int id, SubCriteriaDto subCriteriaDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != subCriteriaDto.ID)
            {
                return BadRequest();
            }
            var subCriteria = DtoToEntityIMapper.Map<SubCriteriaDto, SubCriteria>(subCriteriaDto); ////
            UoW.GetRepository<SubCriteria>().ModifyEntityState(subCriteria);

            try
            {
                await UoW.SaveAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SubCriteriaExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/SubCriterias
        [ResponseType(typeof(SubCriteria))]
        public async Task<IHttpActionResult> PostSubCriteria(SubCriteriaDto subCriteriaDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var subCriteria = DtoToEntityIMapper.Map<SubCriteriaDto, SubCriteria>(subCriteriaDto); ////
            
            UoW.GetRepository<SubCriteria>().Insert(subCriteria);
            await UoW.SaveAsync();

            return CreatedAtRoute("DefaultApi", new { id = subCriteria.ID }, subCriteria);
        }

        // DELETE: api/SubCriterias/5
        [ResponseType(typeof(SubCriteria))]
        public async Task<IHttpActionResult> DeleteSubCriteria(int id)
        {
            SubCriteria subCriteria = await UoW.GetRepository<SubCriteria>().GetItemAsycn(e => e.ID == id);
            if (subCriteria == null)
            {
                return NotFound();
            }

            UoW.GetRepository<SubCriteria>().Delete(subCriteria);
            await UoW.SaveAsync();

            return Ok(subCriteria);
        }


        private bool SubCriteriaExists(int id)
        {
            return UoW.GetRepository<SubCriteria>().GetItem(e => e.ID == id) != null;
        }


    }
}
