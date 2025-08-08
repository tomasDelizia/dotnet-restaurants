using Restaurants.Domain.Constants;

namespace Restaurants.Domain.Exceptions;

public class ForbiddenException(
    string resourceType,
    string resourceId,
    ResourceOperation resourceOperation
    ) : Exception($"{resourceOperation} operation on {resourceType} with id {resourceId} not allowed")
{
}
