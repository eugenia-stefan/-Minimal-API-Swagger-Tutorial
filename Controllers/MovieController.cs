using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MinimalJwt.Models;
using MinimalJwt.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace MinimalJwt.Controllers
{
    [ApiController]
    [Route("movie")]
    public class MovieController : Controller
    {
        private readonly IMovieService service;

        public MovieController(IMovieService service)
        {
            this.service = service;
        }




        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrator")]
        [HttpPost]
        public IResult Create(Movie movie)
        {
            var result = service.Create(movie);
            return Results.Ok(result);
        }


        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Standard, Administrator" )]
        [HttpGet]
        [Route("{id?}")]
        public IResult Get(int id)
        {
            var movie = service.Get(id);

            if (movie is null) return Results.NotFound("Movie not found");

            return Results.Ok(movie);

        }
        [HttpGet]
        public IResult List()
        {
            var movies = service.List();

            return Results.Ok(movies);
        }



        /// <summary>
        /// [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrator")]
        /// </summary>
        /// <param name="newMovie"></param>
        /// <returns></returns>
        [HttpPut]
        public IResult Update(Movie newMovie)
        {
            var updatedMovie = service.Update(newMovie);

            if (updatedMovie is null) Results.NotFound("Movie not found");

            return Results.Ok(updatedMovie);
        }

        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrator")]
        [HttpDelete]
        public IResult Delete(int id, IMovieService service)
        {
            var result = service.Delete(id);

            if (!result) Results.BadRequest("Something went wrong");

            return Results.Ok(result);
        }

        
    }


}
