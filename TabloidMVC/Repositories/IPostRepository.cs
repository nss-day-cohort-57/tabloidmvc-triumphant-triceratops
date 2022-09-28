﻿using System.Collections.Generic;
using TabloidMVC.Models;

namespace TabloidMVC.Repositories
{
    public interface IPostRepository
    {
        void Add(Post post);
        List<Post> GetAllPublishedPosts();
        Post GetPublishedPostById(int id);
        Post GetUserPostById(int id, int userProfileId);
        void UpdatePost(Post post);
        void Delete(int postId);

        List<Post> GetAllPostsByUserId(int userProfileId);
    }
}