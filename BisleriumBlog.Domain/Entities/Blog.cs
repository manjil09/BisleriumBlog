﻿using BisleriumBlog.Domain.Shared;
using System.ComponentModel.DataAnnotations;

namespace BisleriumBlog.Domain.Entities
{
    public class Blog: BaseEntity
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public string Image { get; set; }
        public int UserId { get; set; }
        public List<Comment> Comments{ get; set; } //referencing one to many relation with comment entity
        public List<BlogReaction> Reactions { get; set; } //referencing one to many relation with blogReaction entity

        //pending reference to user table 
    }
}
