using GraphQL;
using GraphQL.Client;
using GraphQL.Client.Http;

using System;
using System.Threading.Tasks;
using Arbook_tools.Model;
using GraphQL.Client.Serializer.Newtonsoft;
using System.Windows;
using Newtonsoft.Json.Linq;
using GraphQL.Client.Abstractions;
using System.Net.Http.Headers;
using System.Net.Http;
using OfficeOpenXml.FormulaParsing.LexicalAnalysis;
using System.Xml.Linq;
using System.Net.Mime;
using System.Collections.Specialized;
using System.Web;

namespace Arbook_tools.Services
{
    public class GraphQLService : GraphQLHttpRequest
    {
        private readonly GraphQLHttpClient _graphQLClient;

        public GraphQLService(string endpointUrl)
        {
            _graphQLClient = new GraphQLHttpClient(endpointUrl, new NewtonsoftJsonSerializer()); 
        }

        public async Task<string> LoginAuth(string email, string password, string isFrom)
        {

            var mutation = new GraphQLRequest
            {
                Query = @"
                     mutation login($email: String!, $password: String!, $isFrom: isFromEnum!) {
                      login(email: $email, password: $password, isFrom: $isFrom) {
                        token
                      }
                    }
                 ",
                OperationName = "login",
                Variables = new
                {
                    email = email,
                    password = password,
                    isFrom = isFrom
                }
            };

            var response = await _graphQLClient.SendMutationAsync<dynamic>(mutation);
            
            if (response.Errors != null)
            {
                // Обработка ошибок, если они есть
                foreach (var error in response.Errors)
                {
                    Console.WriteLine($"GraphQL Error: {error.Message}");
                }

                return null;
            }
            else
            {
                var token = response?.Data?.login?.token;
                return token;
            }
        }

        public async Task<dynamic> GetAplicationMain(string token)
        {
            var request = new GraphQLHttpRequestWithAuthSupport
            {
                Query = @"
                    query GetApplicationMainsByTeacherId($limit: Int, $skip: Int, $teacherId: String!) {
                      getApplicationMainsByTeacherId(limit: $limit, skip: $skip, teacherId: $teacherId) {
                        total
                        applicationsMain {
                          id
                          name
                        }
                      }
                    }
                 ",
                OperationName = "GetApplicationMainsByTeacherId",
                Variables = new
                {
                    skip = 0,
                    limit = 500,
                    teacherId = "7d79668c-a7b2-482d-b667-63380ca78eae"
                },
                Authentication = new AuthenticationHeaderValue(@$"{token}")
            };

            var response = await _graphQLClient.SendQueryAsync<dynamic>(request);

            if (response.Errors != null)
            {
                // Обработка ошибок, если они есть
                foreach (var error in response.Errors)
                {
                    Console.WriteLine($"GraphQL Error: {error.Message}");
                }

                return null;
            }
            else
            {
                // Обработка успешного ответа
                return response.Data;
            }
        }

        public async Task<dynamic> GetEducationPlans(string token, string currentLessonId)
        {
            var request = new GraphQLHttpRequestWithAuthSupport
            {
                Query = @"
                   query GetEducationPlans($data: GetEducationPlansInput!) {
                      getEducationPlans(data: $data) {
                        educationPlans {
                          id
                          name
                        }
                      }
                    }
                 ",
                OperationName = "GetEducationPlans",
                Variables = new
                {
                    data = new
                    {
                        schoolId = "7414d617-c2f4-420a-855c-1c34d205cff1",
                        searchKey = "",
                        mainId = currentLessonId,
                        skip = 0,
                        limit = 100,
                        teacherId = "7d79668c-a7b2-482d-b667-63380ca78eae"
                    }
                },
                Authentication = new AuthenticationHeaderValue(@$"{token}")
            };

            var response = await _graphQLClient.SendQueryAsync<dynamic>(request);

            if (response.Errors != null)
            {
                // Обработка ошибок, если они есть
                foreach (var error in response.Errors)
                {
                    Console.WriteLine($"GraphQL Error: {error.Message}");
                }

                return null;
            }
            else
            {
                // Обработка успешного ответа
                return response.Data;
            }
        }

        public async Task<dynamic> GetEducationPlan(string token, string educationPlanId)
        {
            var request = new GraphQLHttpRequestWithAuthSupport
            {
                Query = @"query GetEducationPlanChapters($educationPlanId: String!, $limit: Int) {
                      getEducationPlanChapters(educationPlanId: $educationPlanId, limit: $limit) {
                        total
                        educationPlanChapters {
                          name
                          id
                          createdById
                          newLessons {
                            id
                          }
                        }
                      }
                    }
                 ",
                OperationName = "GetEducationPlanChapters",
                Variables = new
                {
                    educationPlanId = educationPlanId,
                    limit = 1000
                },
                Authentication = new AuthenticationHeaderValue(@$"{token}")
            };

            var response = await _graphQLClient.SendQueryAsync<dynamic>(request);

            if (response.Errors != null)
            {
                // Обработка ошибок, если они есть
                foreach (var error in response.Errors)
                {
                    Console.WriteLine($"GraphQL Error: {error.Message}");
                }

                return null;
            }
            else
            {
                // Обработка успешного ответа
                return response.Data;
            }
        }

        public async Task<string> CreateEducationPlan(string currentPlanId, string newSestion, string token)
        {
            var mutation = new GraphQLHttpRequestWithAuthSupport
            {
                Query = @"
                     mutation CreateEducationPlanChapter($name: String!, $educationPlanId: String!) {
                      createEducationPlanChapter(name: $name, educationPlanId: $educationPlanId) {
                        id
                      }
                    }
                 ",
                OperationName = "CreateEducationPlanChapter",
                Variables = new
                {
                    educationPlanId = currentPlanId,
                    name = newSestion
                },
                Authentication = new AuthenticationHeaderValue(@$"{token}")
            };

            var response = await _graphQLClient.SendMutationAsync<dynamic>(mutation);

            if (response.Errors != null)
            {
                // Обработка ошибок, если они есть
                foreach (var error in response.Errors)
                {
                    Console.WriteLine($"GraphQL Error: {error.Message}");
                }

                return null;
            }
            else
            {
                var id = response?.Data?.createEducationPlanChapter?.id;
                return id;
            }
        }


        public async Task<string> CreateNewLesson(string currentSectionId, string name, string token)
        {
            var mutation = new GraphQLHttpRequestWithAuthSupport
            {
                Query = @"
                    mutation CreateNewLesson($name: String!, $inAppPurchases: InAppPurchasesEnum, $educationPlanChapterId: String) {
                      createNewLesson(name: $name, inAppPurchases: $inAppPurchases, educationPlanChapterId: $educationPlanChapterId) {
                        id
                      }
                    }
                 ",
                OperationName = "CreateNewLesson",
                Variables = new
                {
                    educationPlanChapterId = currentSectionId,
                    name = name,
                    inAppPurchases = "FREE"
                },
                Authentication = new AuthenticationHeaderValue(@$"{token}")
            };

            var response = await _graphQLClient.SendMutationAsync<dynamic>(mutation);

            if (response.Errors != null)
            {
                // Обработка ошибок, если они есть
                foreach (var error in response.Errors)
                {
                    Console.WriteLine($"GraphQL Error: {error.Message}");
                }

                return null;
            }
            else
            {
                var id = response?.Data?.createNewLesson?.id;
                return id;
            }
        }

        public async Task<string> CreateSlide(string lessonId, string token)
        {
            var mutation = new GraphQLHttpRequestWithAuthSupport
            {
                Query = @"
                    mutation CreateSlide($lessonId: String!, $startPosition: Int!, $slideData: SlideInput) {
                      createSlide(lessonId: $lessonId, startPosition: $startPosition, slideData: $slideData) {
                        id
                      }
                    }
                 ",
                OperationName = "CreateSlide",
                Variables = new
                {
                    lessonId = lessonId,
                    slideData = new {
                        contentType = "VIDEO",
                        videoUrl = (string)null,
                        name = (string)null,
                        image = (string)null,
                        imageName = (string)null,
                        experimentHtml = (string)null,
                        experimentUrl = (string)null,
                        thumbnailUrl = (string)null,
                        testChoices = new string[] { }
                    },
                 startPosition = 0
                },
                Authentication = new AuthenticationHeaderValue(@$"{token}")
            };

            var response = await _graphQLClient.SendMutationAsync<dynamic>(mutation);

            if (response.Errors != null)
            {
                // Обработка ошибок, если они есть
                foreach (var error in response.Errors)
                {
                    Console.WriteLine($"GraphQL Error: {error.Message}");
                }

                return null;
            }
            else
            {
                var id = response?.Data?.createSlide?.id;
                return id;
            }
        }

        public async Task<string> UpdateSlide(string slideId, string link, string token)
        {
            string youtubeLink = link;
            Uri uri = new Uri(youtubeLink);
            string queryString = uri.Query;

            NameValueCollection queryParameters = HttpUtility.ParseQueryString(queryString);
            string videoId = queryParameters["v"];

            string videoUrl = $"https://www.youtube.com/embed/{videoId}";
            string thumbnailUrl = $"http://img.youtube.com/vi/${videoId}";

            var mutation = new GraphQLHttpRequestWithAuthSupport
            {
                Query = @"
                   mutation UpdateSlide($updateSlideId: String!, $slideData: SlideInput, $order: Int) {
                      updateSlide(id: $updateSlideId, slideData: $slideData, order: $order) {
                        id
                      }
                    }
                 ",
                OperationName = "UpdateSlide",
                Variables = new
                {
                    updateSlideId = slideId,
                    slideData = new {
                    contentType = "VIDEO",
                    videoUrl = videoUrl,
                    thumbnailUrl = thumbnailUrl,
                    name = (string)null,
                    image = (string)null,
                    imageName = (string)null,
                    experimentHtml = (string)null,
                    experimentUrl = (string)null
              },
                    order = 0
                },
                Authentication = new AuthenticationHeaderValue(@$"{token}")
            };

            var response = await _graphQLClient.SendMutationAsync<dynamic>(mutation);

            if (response.Errors != null)
            {
                // Обработка ошибок, если они есть
                foreach (var error in response.Errors)
                {
                    Console.WriteLine($"GraphQL Error: {error.Message}");
                }

                return null;
            }
            else
            {
                var id = response?.Data?.updateSlide?.id;
                return id;
            }
        }

        public async Task<bool> UploadNewLessonToMarket(string lessonId, string name, string contentAuthor, string currentLessonId, string desc, string token)
        {
            var mutation = new GraphQLHttpRequestWithAuthSupport
            {
                Query = @"
                  mutation UploadNewLessonToMarket($data: LessonContentInput!) {
                      uploadNewLessonToMarket(data: $data)
                    }
                 ",
                OperationName = "UploadNewLessonToMarket",
                Variables = new
                {
                    data = new {
                        lessonId = lessonId,
                        contentName = name,
                        contentAuthor = contentAuthor,
                        mainIds = new string[] { currentLessonId },
                        schoolGradeIds = new string[]
                        {
                            "4b4f65b8-b3ef-47d8-9723-49d7964fbd5b",
                            "7a03a303-b283-4c3a-ae2e-ff748dd4dfb9",
                            "bdf3d803-2f79-4b32-8d50-673fa324e97a",
                            "bc42e682-2360-4ffb-b3bc-3d8b73cbd0d6",
                            "1eedd9bb-2651-4308-9c51-a79fdee41246",
                            "68052eba-b276-4a79-b721-f593d8f64b1f",
                            "44225e0b-2b24-46a6-81fc-286808e39a38",
                            "5fd67758-a483-43d1-878d-abc7468d8c49",
                            "c343104a-fab8-4143-8c29-2e7e4f195248",
                            "2030a2bf-04ac-4040-b39b-2b4d643fe7b4",
                            "45a1029f-2da9-4aea-a904-05263a4faef9"
                        },
                        contentType = "VIDEO",
                        inAppPurchases = "FREE",
                        contentDescription = desc,
                        teachingFormat = "MIXED",
                        lang = "UKR"
                    }
        },
                Authentication = new AuthenticationHeaderValue(@$"{token}")
            };

            var response = await _graphQLClient.SendMutationAsync<dynamic>(mutation);

            if (response.Errors != null)
            {
                // Обработка ошибок, если они есть
                foreach (var error in response.Errors)
                {
                    Console.WriteLine($"GraphQL Error: {error.Message}");
                }

                return false;
            }
            else
            {
                return response?.Data?.uploadNewLessonToMarket;
            }
        }



    }
}


public class GraphQLHttpRequestWithAuthSupport : GraphQLHttpRequest
    {
        public AuthenticationHeaderValue? Authentication { get; set; }

        public override HttpRequestMessage ToHttpRequestMessage(GraphQLHttpClientOptions options, IGraphQLJsonSerializer serializer)
        {
            var r = base.ToHttpRequestMessage(options, serializer);
            r.Headers.Authorization = Authentication;
            return r;
        }
    }


