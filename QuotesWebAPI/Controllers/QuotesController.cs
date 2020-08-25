using Microsoft.AspNet.Identity;
using QuotesWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApi.OutputCache.V2;

namespace QuotesWebAPI.Controllers
{
    [Authorize]
    public class QuotesController : ApiController
    {
        ApplicationDbContext quotesDbContext = new ApplicationDbContext();
        // GET: api/Quotes
        [AllowAnonymous] //allows public access to this method
        [HttpGet]
        [CacheOutput(ClientTimeSpan = 60, ServerTimeSpan = 60)]
        public IHttpActionResult Get(string sort) //gives Get method the ability to sort quotes
        {
            IQueryable<Quote> quotes;
            switch(sort)
            {
                case "desc":
                   quotes = quotesDbContext.Quotes.OrderByDescending(q => q.CreatedAt);
                    break;
                case "asc":
                    quotes = quotesDbContext.Quotes.OrderBy(q => q.CreatedAt);
                    break;
                default:
                    quotes = quotesDbContext.Quotes;
                    break;
            }
            return Ok(quotes);
        }

        [HttpGet]
        [Route("api/Quotes/MyQuotes")]
        public IHttpActionResult MyQuotes() //method gets the quotes associated with a certain UserID
        {
            string userId = User.Identity.GetUserId();
            var quotes = quotesDbContext.Quotes.Where(q => q.UserID == userId);
            return Ok(quotes);
        }

        [HttpGet] //custom method mapped to Get verb
        [Route("api/Quotes/PagingQuote/{pageNumber=1}/{pageSize=5}")] //attribute routing, needed because PagingQuote is mapped to Get verb
        public IHttpActionResult PagingQuote(int pageNumber, int pageSize)
        {
            var quotes = quotesDbContext.Quotes.OrderBy(q => q.ID);
            return Ok(quotes.Skip((pageNumber - 1) * pageSize).Take(pageSize));
        }

        [HttpGet]
        [Route("api/Quotes/SearchQuote/{type=}")]
        public IHttpActionResult SearchQuote(string type) //method searches for a quote of a specific type
        {
            var quotes = quotesDbContext.Quotes.Where(q => q.Type.StartsWith(type));
            return Ok(quotes);
        }

        // GET: api/Quotes/5
        public IHttpActionResult Get(int id)
        {
            var quote = quotesDbContext.Quotes.Find(id);
            if (quote == null)
            {
                return NotFound();
            }
            return Ok(quote);
        }

        // POST: api/Quotes
        public IHttpActionResult Post([FromBody]Quote quote)
        {
            string userId = User.Identity.GetUserId(); //gets specific ID of user sending the request with an access token and stores it in userId variable
            quote.UserID = userId; //assigns userID to the UserID property of Quotes class

            if(!ModelState.IsValid) //model validations to ensure correct format
            {
                return BadRequest(ModelState);
            }
            quotesDbContext.Quotes.Add(quote);          
            quotesDbContext.SaveChanges(); //after adding quote, make sure to save changes to database
            return StatusCode(HttpStatusCode.Created);
        }

        // PUT: api/Quotes/5
        public IHttpActionResult Put(int id, [FromBody]Quote quote)
        {
            if (!ModelState.IsValid) //model validations to ensure correct format
            {
                return BadRequest(ModelState);
            }
            string userId = User.Identity.GetUserId();
            var entity = quotesDbContext.Quotes.FirstOrDefault(q => q.ID == id); //gets any record from the database

            if(entity == null)
            {
                return BadRequest("No record found with this ID");
            }

            if (userId != entity.UserID) //checks to make sure the user trying to access record matches the user who owns the record in the database
            {
                return BadRequest("You do not have the right to update this record...");
            }

            entity.Title = quote.Title;
            entity.Author = quote.Author;
            entity.Description = quote.Description;
            quotesDbContext.SaveChanges();
            return Ok("Record updated successfully");
        }

        // DELETE: api/Quotes/5
        public IHttpActionResult Delete(int id)
        {
            string userId = User.Identity.GetUserId();
            var quote = quotesDbContext.Quotes.Find(id);
            if (quote == null)
            {
                return NotFound();
            }

            if (userId != quote.UserID)
            {
                return BadRequest("You do not have the right to delete this record...");
            }

            quotesDbContext.Quotes.Remove(quote);
            quotesDbContext.SaveChanges();
            return Ok("Quote deleted");
        }
    }
}
