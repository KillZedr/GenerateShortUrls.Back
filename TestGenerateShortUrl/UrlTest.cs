using GenerateShortUrls.BLL.Contracts;
using GenerateShortUrls.BLL.RealizationServiceInterface;
using GenerateShortUrsl.Data.GenerateShortUrls.DAL.Contracts;
using GenerateShortUrsl.Data.Identity;
using GenerateShortUrsl.Data.Url;
using MockQueryable.Moq;
using Moq;

namespace TestGenerateShortUrl
{
    public class UrlTest
    {
        [Fact]
        public async Task CreateShortUrlAsync_ShouldReturnShortUrl_WhenOriginalUrlIsValid()
        {
            // Arrange
            var originalUrl = "https://example.com";
            var mockUrlMappings = new List<UrlMapping>().AsQueryable().BuildMockDbSet();
            var urlRepositoryMock = new Mock<IUrlRepository<UrlMapping>>();
            var userRepositoryMock = new Mock<IUrlRepository<User>>();

            urlRepositoryMock.Setup(repo => repo.AsQueryable()).Returns(mockUrlMappings.Object);
            urlRepositoryMock.Setup(repo => repo.Create(It.IsAny<UrlMapping>())).Verifiable();
            urlRepositoryMock.Setup(repo => repo.SaveChangesAsync()).ReturnsAsync(1);

            var urlService = new UrlService(urlRepositoryMock.Object, userRepositoryMock.Object);

            // Act
            var shortUrl = await urlService.CreateShortUrlAsync(originalUrl);

            // Assert
            Assert.NotNull(shortUrl);
            Assert.StartsWith("https://localhost:7107/", shortUrl);
            urlRepositoryMock.Verify(repo => repo.Create(It.IsAny<UrlMapping>()), Times.Once);
            urlRepositoryMock.Verify(repo => repo.SaveChangesAsync(), Times.Once);
        }


        [Fact]
        public async Task CreateShortUrlAsync_ShouldReturnExistingShortUrl_WhenOriginalUrlAlreadyExists()
        {
            // Arrange
            var originalUrl = "https://example.com";

           
            var mockUrlMappings = new List<UrlMapping>
                {
                    new UrlMapping
                    {
                        OriginalUrl = originalUrl,
                        ShortenedUrl = "https://localhost:7107/abc123"
                    }
                }.AsQueryable().BuildMockDbSet();

            var urlRepositoryMock = new Mock<IUrlRepository<UrlMapping>>();
            var userRepositoryMock = new Mock<IUrlRepository<User>>();

          
            urlRepositoryMock.Setup(repo => repo.AsQueryable())
                             .Returns(mockUrlMappings.Object);

            var urlService = new UrlService(urlRepositoryMock.Object, userRepositoryMock.Object);

            // Act
            var shortUrl = await urlService.CreateShortUrlAsync(originalUrl);

            // Assert
            Assert.Equal(mockUrlMappings.Object.First().ShortenedUrl, shortUrl);
            urlRepositoryMock.Verify(repo => repo.Create(It.IsAny<UrlMapping>()), Times.Never);
            urlRepositoryMock.Verify(repo => repo.SaveChangesAsync(), Times.Never);
        }

        [Fact]
        public async Task CreateShortUrlAsync_ShouldThrowException_WhenOriginalUrlIsEmpty()
        {
            // Arrange
            var urlService = new UrlService(Mock.Of<IUrlRepository<UrlMapping>>(), Mock.Of<IUrlRepository<User>>());

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentException>(() => urlService.CreateShortUrlAsync(""));
            Assert.Equal("Original URL cannot be null or empty.", exception.Message);
        }

        [Fact]
        public async Task GetOriginalUrlAsync_ShouldReturnOriginalUrl_WhenShortUrlIsValid()
        {
            // Arrange
            var shortUrl = "https://localhost:7107/abc123";
            var originalUrl = "https://example.com";
            var urlMapping = new UrlMapping
            {
                ShortenedUrl = shortUrl,
                OriginalUrl = originalUrl
            };

            
            var mockUrlMappings = new List<UrlMapping> { urlMapping }.AsQueryable().BuildMockDbSet();

            var urlRepositoryMock = new Mock<IUrlRepository<UrlMapping>>();
            var userRepositoryMock = new Mock<IUrlRepository<User>>();

          
            urlRepositoryMock.Setup(repo => repo.AsQueryable())
                             .Returns(mockUrlMappings.Object);

            var urlService = new UrlService(urlRepositoryMock.Object, userRepositoryMock.Object);

            // Act
            var result = await urlService.GetOriginalUrlAsync(shortUrl);

            // Assert
            Assert.Equal(originalUrl, result);
        }

        [Fact]
        public async Task GetOriginalUrlAsync_ShouldThrowException_WhenShortUrlNotFound()
        {
            // Arrange
            var mockUrlMappings = new List<UrlMapping>().AsQueryable().BuildMockDbSet();

            var urlRepositoryMock = new Mock<IUrlRepository<UrlMapping>>();
            var userRepositoryMock = new Mock<IUrlRepository<User>>();

            
            urlRepositoryMock.Setup(repo => repo.AsQueryable())
                             .Returns(mockUrlMappings.Object);

            var urlService = new UrlService(urlRepositoryMock.Object, userRepositoryMock.Object);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() => urlService.GetOriginalUrlAsync("https://localhost:7107/abc123"));
            Assert.Equal("Short URL not found.", exception.Message);
        }
    }

}