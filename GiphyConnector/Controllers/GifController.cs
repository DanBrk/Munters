using GiphyConnectorService.Dtos;
using GiphyConnectorService.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace GiphyConnector.Controllers
{
    [ApiController]
    [Route("v1/gif")]
    public class GifController : ControllerBase
    {
        private readonly ILogger<GifController> _logger;
        private readonly IGifService _gifContentService;

        public GifController(ILogger<GifController> logger, IGifService gifContentService)
        {
            _logger = logger;
            _gifContentService = gifContentService;
        }

        /// <summary>
        /// Fetch the URLs of the trending GIFs of the day
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("trending")]
        public async Task<ActionResult<GifQueryResponse>> GetTrendingGifsData([FromQuery] int from = 0, [FromQuery] int size = 25)
        {
            try
            {
                return await _gifContentService.FetchTrendings(from, size);
            }
            catch (Exception ex)
            {
                return UnhannledException("GetTrendingGifsUrls",ex);
            }
        }

        /// <summary>
        //Receives a search term as input, and will return the URLs of each GIF found/ 
        /// </summary>
        /// <param name="searchTerm"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<GifQueryResponse>> SearchGifsUrls([FromQuery] string searchTerm, [FromQuery] int from = 0, [FromQuery] int size=25)
        {
            try
            {
                return await _gifContentService.Search(searchTerm, from, size);
            }
            catch (Exception ex)
            {               
                return UnhannledException("SearchGifsUrls", ex);
            }
        }

        private ActionResult<GifQueryResponse> UnhannledException(string description, Exception ex)
        {
            _logger.LogError($"{description} failed. {ex}");

            return StatusCode(StatusCodes.Status500InternalServerError,
                new GifQueryResponse
                {
                    IsOk = false,
                    Message = $"{description} failed with unexpected error. {ex.Message}"
                });
        }

    }
}

