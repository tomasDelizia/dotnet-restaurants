using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Interfaces;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Restaurants.Commands.UploadRestaurantLogo;

public class UploadRestaurantLogoCommandHandler(
    IRestaurantsRepository restaurantsRepository,
    IRestaurantAuthorizationService restaurantAuthorizationService,
    IBlobStorageService blobStorageService,
    ILogger<UploadRestaurantLogoCommandHandler> logger
) : IRequestHandler<UploadRestaurantLogoCommand>
{
    public async Task Handle(UploadRestaurantLogoCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Updating logo for restaurant of id {RequestId} with {@Request}", request.RestaurantId, request);
        var restaurant = await restaurantsRepository.GetByIdAsync(request.RestaurantId) ?? throw new NotFoundException(nameof(Restaurant), request.RestaurantId.ToString());
        if (!restaurantAuthorizationService.Authorize(restaurant, ResourceOperation.Update))
        {
            throw new ForbiddenException(nameof(Restaurant), request.RestaurantId.ToString(), ResourceOperation.Update);
        }
        var logoUrl = await blobStorageService.UploadToBlobStorageAsync(request.FileName, request.File);
        restaurant.LogoUrl = logoUrl;
        await restaurantsRepository.SaveChangesAsync();
    }
}
