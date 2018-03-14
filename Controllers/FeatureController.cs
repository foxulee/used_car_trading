using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using project_vega.Controllers.Resources;
using project_vega.Core.Models;
using project_vega.Persistence;

namespace project_vega.Controllers
{
    [Produces("application/json")]
    [Route("api/Features")]
    public class FeatureController : Controller
    {
        private readonly VegaDbContext _dbContext;
        private readonly IMapper _mapper;

        public FeatureController(VegaDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IEnumerable<KeyValuePairResource>> GetFeatures()
        {
            var features =  await _dbContext.Features.ToListAsync();
            return _mapper.Map<IEnumerable<Feature>, IEnumerable<KeyValuePairResource>>(features);
        }
    }
}