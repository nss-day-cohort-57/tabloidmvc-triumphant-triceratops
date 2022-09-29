using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;
using TabloidMVC.Models;
using TabloidMVC.Utils;

namespace TabloidMVC.Repositories
{
    public class CommentRepository : BaseRepository, ICommentRepository
    {
        public CommentRepository(IConfiguration config) : base(config) { }
        public void Add(Comment comment)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        INSERT INTO Comment (
                            PostId, UserProfileId, Subject, Content, CreateDateTime )
                        OUTPUT INSERTED.ID
                        VALUES ( @PostId, @UserProfileId,
                            @Subject, @Content, @CreateDateTime )";
                    cmd.Parameters.AddWithValue("@PostId", comment.PostId);
                    cmd.Parameters.AddWithValue("@UserProfileId", comment.UserProfileId);
                    cmd.Parameters.AddWithValue("@Subject", comment.Subject);
                    cmd.Parameters.AddWithValue("@Content", comment.Content);
                    cmd.Parameters.AddWithValue("@CreateDateTime", comment.CreateDateTime);

                    comment.Id = (int)cmd.ExecuteScalar();
                }
            }
        }

        public List<Comment> GetCommentsByPostId(int postId)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                                        SELECT c.Id, c.Subject, c.Content, c.CreateDateTime, c.UserProfileId
                                        FROM Comment c
                                        JOIN UserProfile u on u.Id = c.UserProfileId WHERE c.PostId = @postId";
                    cmd.Parameters.AddWithValue("@postId", postId);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        List<Comment> comments = new List<Comment>();

                        while (reader.Read())
                        {
                            Comment comment = new Comment()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Subject = reader.GetString(reader.GetOrdinal("Subject")),
                                Content = reader.GetString(reader.GetOrdinal("Content")),
                                CreateDateTime = reader.GetDateTime(reader.GetOrdinal("CreateDateTime")),
                                UserProfileId = reader.GetInt32(reader.GetOrdinal("UserProfileId"))
                            };

                            comments.Add(comment);
                        }

                        return comments;
                    }
                }
            }
        }
    }
}


       
                