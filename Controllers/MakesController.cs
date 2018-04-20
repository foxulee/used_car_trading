using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using project_vega.Controllers.Resources;
using project_vega.Core.Models;
using project_vega.Persistence;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace project_vega.Controllers
{
    [Produces("application/json")]
    [Route("api/Makes")]
    public class MakesController : Controller
    {

        //Dependency inject for using DbContext
        private readonly VegaDbContext _dbContext;
        private readonly IMapper _mapper;

        public MakesController(VegaDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IEnumerable<MakeResource>> GetMakes()
        {
            //Include(): including navigation property
            //Because we don't apply lazy load to the model (no virtual keyword for the navigation property)
            var makes = await _dbContext.Makes.Include(m => m.Models).ToListAsync();
            return _mapper.Map<List<Make>, List<MakeResource>>(makes);
        }
    }
}