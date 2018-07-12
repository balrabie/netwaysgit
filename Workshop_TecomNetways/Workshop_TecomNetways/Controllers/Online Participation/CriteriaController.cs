using AutoMapper;
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

    public class CriteriaController : ApiController
    {
        private UnitOfWork UoW = null;
        private IMapper EntityToDtoIMapper = null;
        private IMapper DtoToEntityIMapper = null;

        public CriteriaController()
        {
            UoW = new UnitOfWork();
            InitializeMapping();

        }

        public CriteriaController(UnitOfWork uoW)
        {
            UoW = uoW;
            InitializeMapping();
        }

        private void InitializeMapping()
        {
            EntityToDtoIMapper = new MapperConfiguration
                (cfg => cfg.CreateMap<Criteria, CriteriaDto>())
                .CreateMapper();
            DtoToEntityIMapper = new MapperConfiguration
                (cfg => cfg.CreateMap<CriteriaDto, Criteria>())
                .CreateMapper();
        }

        // GET: api/Criterias
        [HttpGet]
        public IHttpActionResult GetAll()
        {
            var items = UoW.GetRepository<Criteria>().GetAll().ToList();
            var DTO = EntityToDtoIMapper.Map<List<Criteria>, List<CriteriaDto>>(items).ToList();

            return Ok(DTO);
        }

        // GET: api/Criterias/5
        [ResponseType(typeof(Criteria))]
        public async Task<IHttpActionResult> GetCriteria(int id)
        {
            Criteria item = await UoW.GetRepository<Criteria>().GetItemAsycn(e => e.ID == id);
            if (item == null)
            {
                return NotFound();
            }

            var DTO = EntityToDtoIMapper.Map<Criteria, CriteriaDto>(item);
            return Ok(DTO);
        }


        // POST: api/Criterias
        [ResponseType(typeof(Criteria))]
        public async Task<IHttpActionResult> PostCriteria(CriteriaDto criteriaDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var criteria = DtoToEntityIMapper.Map<CriteriaDto, Criteria>(criteriaDto); ////
            UoW.GetRepository<Criteria>().Insert(criteria);
            await UoW.SaveAsync();

            return CreatedAtRoute("DefaultApi", new { id = criteria.ID }, criteria);
        }

        // DELETE: api/Criterias/5
        [ResponseType(typeof(Criteria))]
        public async Task<IHttpActionResult> DeleteCriteria(int id)
        {
            Criteria criteria = await UoW.GetRepository<Criteria>().GetItemAsycn(e => e.ID == id);
            if (criteria == null)
            {
                return NotFound();
            }

            UoW.GetRepository<Criteria>().Delete(criteria);
            await UoW.SaveAsync();

            return Ok(criteria);
        }



        private bool CriteriaExists(int id)
        {
            return UoW.GetRepository<Criteria>().GetItem(e => e.ID == id) != null;
        }
    }
}
