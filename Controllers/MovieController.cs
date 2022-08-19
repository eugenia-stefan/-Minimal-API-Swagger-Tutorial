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
    [Route("controller")]
    public class MovieController : IMovieService
    {
        private readonly IMovieService service;

        public MovieController(IMovieService service)
        {
            this.service = service;
        }


       

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrator")]
        public IResult Create(Movie movie)
        {
            var result = service.Create(movie);
            return Results.Ok(result);
        }

         [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Standard, Administrator" )]
        public IResult Get(int id)
        {
            var movie = service.Get(id);

            if (movie is null) return Results.NotFound("Movie not found");

            return Results.Ok(movie);

        }
        

        public IResult List()
        {
            var movies = service.List();

            return Results.Ok(movies);
        }



        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrator")]

        public IResult Update(Movie newMovie)
        {
            var updatedMovie = service.Update(newMovie);

            if (updatedMovie is null) Results.NotFound("Movie not found");

            return Results.Ok(updatedMovie);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrator")]

        public IResult Delete(int id, IMovieService service)
        {
            var result = service.Delete(id);

            if (!result) Results.BadRequest("Something went wrong");

            return Results.Ok(result);
        }

        
    }


}
