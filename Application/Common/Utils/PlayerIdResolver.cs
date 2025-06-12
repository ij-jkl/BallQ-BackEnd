namespace Application.Common.Utils;

public static class PlayerIdResolver
{
    /// <summary>
    /// Resolves the final list of player IDs by replacing any '0' values with randomly selected player IDs,
    /// this helps to avoid duplicates and retrying up to a defined maximum number of times.
    /// </summary>
    ///
    /// 
    public static async Task<List<int>> ResolveFinalIdsTask(
        List<int> inputIds,
        IPlayerRatingRepository repository,
        int maxRetries = 10)
    {
        // Create a HashSet to make sure to store unique player IDs
        var selectedIds = new HashSet<int>();

        // Loop through the input list of player IDs
        foreach (var id in inputIds)
        {
            if (id == 0)
            {
                RatingEntity? random;
                int retries = 0;

                // Retry selecting a random player until one is found that hasn't already been selected
                do
                {
                    random = await repository.GetRandomPlayerRatings();
                    retries++;
                }
                while (random != null && selectedIds.Contains(random.PlayerId) && retries < maxRetries);

                // If a valid, non-duplicate random player is found, add their ID to the set
                if (random != null && !selectedIds.Contains(random.PlayerId))
                {
                    selectedIds.Add(random.PlayerId);
                }
            }
            else
            {
                // Add explicitly provided player IDs (other than 0)
                selectedIds.Add(id);
            }
        }

        return selectedIds.ToList();
    }
}