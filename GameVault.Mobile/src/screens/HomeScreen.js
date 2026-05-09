import { useEffect, useMemo, useState } from 'react';
import { ActivityIndicator, Pressable, ScrollView, Text, View } from 'react-native';

import GameCard from '../components/GameCard';
import NewsCard from '../components/NewsCard';
import ReviewCard from '../components/ReviewCard';
import SortPicker from '../components/SortPicker';
import { getGames } from '../services/gameService';
import { getNews } from '../services/newsService';
import { getReviewsByGameId } from '../services/reviewService';
import { colors, spacing } from '../theme';

const sortOptions = [
  { label: 'Highest Rating', value: 'rating' },
  { label: 'Release Date', value: 'releasedate' },
  { label: 'Alphabetical', value: 'alphabetical' },
];

function sortGames(games, sort) {
  const list = [...games];

  switch (sort) {
    case 'releasedate':
      return list.sort((left, right) => new Date(right.releaseDate) - new Date(left.releaseDate));
    case 'alphabetical':
      return list.sort((left, right) => left.title.localeCompare(right.title));
    case 'rating':
    default:
      return list.sort((left, right) => {
        const leftRating = left.averageRating ?? -1;
        const rightRating = right.averageRating ?? -1;
        return rightRating - leftRating;
      });
  }
}

export default function HomeScreen({ navigation }) {
  const [games, setGames] = useState([]);
  const [news, setNews] = useState([]);
  const [recentReviews, setRecentReviews] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');
  const [trendingSort, setTrendingSort] = useState('rating');
  const [newReleasesSort, setNewReleasesSort] = useState('releasedate');
  const [reloadTick, setReloadTick] = useState(0);

  useEffect(() => {
    let mounted = true;

    async function loadHome() {
      try {
        setLoading(true);
        setError('');

        const [gamesResponse, newsResponse] = await Promise.all([getGames(), getNews()]);

        if (!mounted) {
          return;
        }

        setGames(gamesResponse ?? []);
        setNews(newsResponse ?? []);

        const reviewGroups = await Promise.allSettled(
          (gamesResponse ?? []).map(async (game) => {
            const reviews = await getReviewsByGameId(game.gameId, 'newest');
            return (reviews ?? []).map((review) => ({
              ...review,
              gameTitle: game.title,
            }));
          }),
        );

        if (!mounted) {
          return;
        }

        const flattenedReviews = reviewGroups
          .flatMap((result) => (result.status === 'fulfilled' ? result.value : []))
          .sort((left, right) => new Date(right.createdAt) - new Date(left.createdAt))
          .slice(0, 6);

        setRecentReviews(flattenedReviews);
      } catch (loadError) {
        if (mounted) {
          setError(loadError.message || 'Failed to load Home.');
        }
      } finally {
        if (mounted) {
          setLoading(false);
        }
      }
    }

    loadHome();

    return () => {
      mounted = false;
    };
  }, [reloadTick]);

  const trendingGames = useMemo(
    () => sortGames(games.filter((game) => game.averageRating !== null), trendingSort).slice(0, 10),
    [games, trendingSort],
  );

  const newReleaseGames = useMemo(() => sortGames(games, newReleasesSort).slice(0, 10), [games, newReleasesSort]);

  if (loading) {
    return (
      <View className="flex-1 items-center justify-center" style={{ backgroundColor: colors.background }}>
        <ActivityIndicator size="large" color={colors.accent} />
      </View>
    );
  }

  if (error) {
    return (
      <View className="flex-1 items-center justify-center px-6" style={{ backgroundColor: colors.background }}>
        <Text className="text-center text-lg font-semibold" style={{ color: colors.text }}>
          {error}
        </Text>
        <Pressable onPress={() => setReloadTick((current) => current + 1)} className="mt-5 rounded-full px-5 py-3" style={{ backgroundColor: colors.accent }}>
          <Text className="font-semibold" style={{ color: '#04120B' }}>
            Retry
          </Text>
        </Pressable>
      </View>
    );
  }

  return (
    <ScrollView style={{ backgroundColor: colors.background }} contentContainerStyle={{ padding: spacing.page, paddingBottom: 120 }}>
      <View className="flex-row items-center justify-between">
        <View>
          <Text className="text-3xl font-bold" style={{ color: colors.text }}>
            GameVault
          </Text>
          <Text className="mt-1 text-sm" style={{ color: colors.muted }}>
            Track the games that matter.
          </Text>
        </View>
        <Pressable onPress={() => navigation.navigate('Login')} className="rounded-full px-4 py-2" style={{ backgroundColor: colors.accent }}>
          <Text className="font-semibold" style={{ color: '#04120B' }}>
            Sign In
          </Text>
        </Pressable>
      </View>

      {news.length ? (
        <View className="mt-6">
          <Text className="mb-3 text-lg font-semibold" style={{ color: colors.text }}>
            Latest News
          </Text>
          <ScrollView horizontal showsHorizontalScrollIndicator={false}>
            {news.map((item) => (
              <NewsCard key={item.newsId} news={item} />
            ))}
          </ScrollView>
        </View>
      ) : null}

      <View className="mt-7">
        <View className="mb-3 flex-row items-center justify-between">
          <Text className="text-lg font-semibold" style={{ color: colors.text }}>
            Trending Games
          </Text>
          <SortPicker options={sortOptions} selected={trendingSort} onSelect={setTrendingSort} />
        </View>
        <ScrollView horizontal showsHorizontalScrollIndicator={false}>
          {trendingGames.map((game) => (
            <GameCard key={game.gameId} game={game} onPress={() => navigation.navigate('GameDetail', { gameId: game.gameId })} />
          ))}
        </ScrollView>
      </View>

      <View className="mt-7">
        <View className="mb-3 flex-row items-center justify-between">
          <Text className="text-lg font-semibold" style={{ color: colors.text }}>
            New Releases
          </Text>
          <SortPicker options={sortOptions} selected={newReleasesSort} onSelect={setNewReleasesSort} />
        </View>
        <ScrollView horizontal showsHorizontalScrollIndicator={false}>
          {newReleaseGames.map((game) => (
            <GameCard key={game.gameId} game={game} onPress={() => navigation.navigate('GameDetail', { gameId: game.gameId })} />
          ))}
        </ScrollView>
      </View>

      {recentReviews.length ? (
        <View className="mt-7">
          <Text className="mb-3 text-lg font-semibold" style={{ color: colors.text }}>
            Recent Reviews
          </Text>
          {recentReviews.map((review) => (
            <ReviewCard key={review.reviewId} review={review} showGameTitle />
          ))}
        </View>
      ) : null}
    </ScrollView>
  );
}