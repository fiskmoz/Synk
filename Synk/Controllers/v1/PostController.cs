using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Synk.Contracts;
using Synk.Contracts.v1;
using Synk.Contracts.v1.Requests;
using Synk.Contracts.v1.Responses;
using Synk.Extensions;
using Synk.Models;
using Synk.Services;

namespace Synk.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PostController : Controller
    {
        private readonly IPostService _postService;
        public PostController(IPostService postService)
        {
            _postService = postService;
        }

        [HttpGet(ApiRoutes.Posts.GetAll, Name = EndpointNames.Posts.GetAll)]
        public async Task<ActionResult<MultiplePostsResponse>> GetAll()
        {
            return Ok(new MultiplePostsResponse { posts = await _postService.GetPostsAsync() });
        }

        [HttpPut(ApiRoutes.Posts.Update, Name = EndpointNames.Posts.Update)]
        public async Task<ActionResult<SinglePostResponse>> Update([FromRoute]Guid postId, [FromBody] UpdatePostRequest request)
        {
            var userOwnsPost = await _postService.UserOwnsPostAsync(postId, HttpContext.GetUserId());
            if (!userOwnsPost)
            {
                return BadRequest(new { error = "You do not own this post" });
            }
            var post = await _postService.GetPostByIdAsync(postId);
            post.Body = request.Body;

            var updated = await _postService.UpdatePostAsync(post);
            if (updated)
                return Ok(new SinglePostResponse { Post = post });
            return NotFound();
        }

        [HttpDelete(ApiRoutes.Posts.Delete, Name = EndpointNames.Posts.Delete)]
        public async Task<IActionResult> Update([FromRoute]Guid postId)
        {
            var userOwnsPost = await _postService.UserOwnsPostAsync(postId, HttpContext.GetUserId());
            if (!userOwnsPost)
            {
                return BadRequest(new { error = "You do not own this post" });
            }
            var deleted = await _postService.DeletePostAsync(postId);
            if (deleted)
                return NoContent();
            return NotFound();
        }

        [HttpGet(ApiRoutes.Posts.Get, Name = EndpointNames.Posts.Get)]
        public async Task<ActionResult<SinglePostResponse>> Get([FromRoute]Guid postId)
        {
            var post = await _postService.GetPostByIdAsync(postId);
            if (post == null)
                return NotFound();
            return Ok(new SinglePostResponse { Post = post });
        }

        [HttpPost(ApiRoutes.Posts.Create, Name = EndpointNames.Posts.Create)]
        public async Task<ActionResult<SinglePostResponse>> Create([FromBody] CreatePostRequest postRequest)
        {
            var post = new Post
            {
                Body = postRequest.Body,
                UserId = HttpContext.GetUserId()
            };

            await _postService.CreatePostAsync(post);

            var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.ToUriComponent()}";
            var locationUri = baseUrl + "/" + ApiRoutes.Posts.Get.Replace("{postId}", post.Id.ToString());

            var response = new SinglePostResponse { Post = post, locationUri = locationUri };
            return Ok(response);
        }
    }
}
