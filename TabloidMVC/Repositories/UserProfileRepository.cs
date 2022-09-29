using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using TabloidMVC.Models;
using TabloidMVC.Utils;

namespace TabloidMVC.Repositories
{
    public class UserProfileRepository : BaseRepository, IUserProfileRepository
    {
        public UserProfileRepository(IConfiguration config) : base(config) { }

        public UserProfile GetByEmail(string email)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                       SELECT u.id, u.FirstName, u.LastName, u.DisplayName, u.Email,
                              u.CreateDateTime, u.ImageLocation, u.UserTypeId,
                              ut.[Name] AS UserTypeName
                         FROM UserProfile u
                              LEFT JOIN UserType ut ON u.UserTypeId = ut.id
                        WHERE email = @email";
                    cmd.Parameters.AddWithValue("@email", email);

                    UserProfile userProfile = null;
                    var reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        userProfile = new UserProfile()
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Email = reader.GetString(reader.GetOrdinal("Email")),
                            FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                            LastName = reader.GetString(reader.GetOrdinal("LastName")),
                            DisplayName = reader.GetString(reader.GetOrdinal("DisplayName")),
                            CreateDateTime = reader.GetDateTime(reader.GetOrdinal("CreateDateTime")),
                            ImageLocation = DbUtils.GetNullableString(reader, "ImageLocation"),
                            UserTypeId = reader.GetInt32(reader.GetOrdinal("UserTypeId")),
                            UserType = new UserType()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("UserTypeId")),
                                Name = reader.GetString(reader.GetOrdinal("UserTypeName"))
                            },
                        };
                    }

                    reader.Close();

                    return userProfile;
                }
            }
        }
        public void AddUserProfile(UserProfile profile)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                                        INSERT  INTO UserProfile (DisplayName, FirstName, LastName, Email, CreateDateTime, ImageLocation, UserTypeId)
                                       OUTPUT INSERTED.ID
                                       
                                        VALUES (@displayName, @firstName, @LastName, @email, @createDateTime, @imageLocation, @userTypeId);";
                    cmd.Parameters.AddWithValue("@displayName", profile.DisplayName);
                    cmd.Parameters.AddWithValue("@firstName", profile.FirstName);
                    cmd.Parameters.AddWithValue("@lastName", profile.LastName);
                    cmd.Parameters.AddWithValue("@email", profile.Email);
                    cmd.Parameters.AddWithValue("@createDateTime", profile.CreateDateTime);
                    cmd.Parameters.AddWithValue("@imageLocation", profile.ImageLocation);
                    cmd.Parameters.AddWithValue("@userTypeId", 2);

                    int id = (int)cmd.ExecuteScalar();

                    profile.Id = id;

                }
            }
        }
        public UserProfile GetUserProfileById(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT  up.Id, up.ImageLocation, up.[FirstName], up.LastName, up.DisplayName, up.Email, up.CreateDateTime, u.Name As Type
                        FROM UserProfile up
                        JOIN UserType u ON u.Id = up.UserTypeId
                        WHERE up.Id = @id";


                    cmd.Parameters.AddWithValue("@id", id);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            UserProfile profile = new UserProfile()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                //ImageLocation = reader.GetString(reader.GetOrdinal("ImaegeLocation")),
                                FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                LastName = reader.GetString(reader.GetOrdinal("LastName")),
                                DisplayName = reader.GetString(reader.GetOrdinal("DisplayName")),
                                Email = reader.GetString(reader.GetOrdinal("Email")),
                                CreateDateTime = reader.GetDateTime(reader.GetOrdinal("CreateDateTime")),

                                UserType = new UserType()
                                {
                                    Name = reader.GetString(reader.GetOrdinal("Type")),
                                }


                            };
                            if (!reader.IsDBNull(reader.GetOrdinal("ImageLocation")))
                            {
                                profile.ImageLocation = reader.GetString(reader.GetOrdinal("ImageLocation"));
                            }
                            return profile;
                        }
                        return null;
                    }

                }
            }

        }
        public List<UserProfile> GetAll()
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT u.Id, u.FirstName, u.LastName, u.DisplayName, u.UserTypeId, ut.Name AS UserTypeName
                                        FROM UserProfile u
                                        JOIN UserType ut ON ut.Id = u.UserTypeId
                                        ORDER BY u.FirstName";

                    using (var reader = cmd.ExecuteReader())
                    {
                        List<UserProfile> userProfile = new List<UserProfile>();
                        while (reader.Read())
                        {
                            UserProfile profile = new UserProfile
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                LastName = reader.GetString(reader.GetOrdinal("LastName")),
                                DisplayName = reader.GetString(reader.GetOrdinal("DisplayName")),
                                UserTypeId = reader.GetInt32(reader.GetOrdinal("UserTypeId")),
                                UserType = new UserType()
                                {
                                    Name = reader.GetString(reader.GetOrdinal("UserTypeName")),
                                }
                            };

                            userProfile.Add(profile);
                        }

                        return userProfile;
                    }
                }
            }
        }
    }
}
