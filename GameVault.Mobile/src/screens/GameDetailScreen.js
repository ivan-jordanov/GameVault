import { useEffect, useMemo, useState } from 'react';
import { ActivityIndicator, Dimensions, Pressable, ScrollView, Text, View } from 'react-native';
import { Image } from 'expo-image';
import dayjs from 'dayjs';

import RatingBadge from '../components/RatingBadge';
import ReviewCard from '../components/ReviewCard';
import SortPicker from '../components/SortPicker';
import { getGameById } from '../services/gameService';
import { getReviewsByGameId } from '../services/reviewService';
import { colors, spacing } from '../theme';

const reviewSortOptions = [
  { label: 'Newest', value: 'newest' },
  { label: 'Oldest', value: 'oldest' },
  { label: 'Highest Rating', value: 'highest' },
  { label: 'Lowest Rating', value: 'lowest' },
];

function chipContainer() {
  return {
    backgroundColor: colors.accentSoft,
    borderColor: colors.border,
  };
}

export default function GameDetailScreen({ route, navigation }) {
  const gameId = route?.params?.gameId;
  const [game, setGame] = useState(null);
  const [reviews, setReviews] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');
  const [reviewSort, setReviewSort] = useState('newest');
  const [descriptionExpanded, setDescriptionExpanded] = useState(false);
  const [descriptionCanToggle, setDescriptionCanToggle] = useState(false);
  const [reloadTick, setReloadTick] = useState(0);
  const [lastLoadedReviewSort, setLastLoadedReviewSort] = useState('');

  useEffect(() => {
    let mounted = true;

    async function loadDetail() {
      try {
        setLoading(true);
        setError('');

        const [gameResponse, reviewResponse] = await Promise.all([
          getGameById(gameId),
          getReviewsByGameId(gameId, reviewSort),
        ]);

        if (!mounted) {
          return;
        }

        setGame(gameResponse);
        setReviews(reviewResponse ?? []);
        setLastLoadedReviewSort(reviewSort);
      } catch (loadError) {
        if (mounted) {
          setError(loadError.message || 'Failed to load game details.');
        }
      } finally {
        if (mounted) {
          setLoading(false);
        }
      }
    }

    if (gameId !== undefined && gameId !== null) {
      loadDetail();
    } else {
      setError('Missing game id.');
      setLoading(false);
    }

    return () => {
      mounted = false;
    };
  }, [gameId, reloadTick]);

  useEffect(() => {
    let mounted = true;

    async function loadReviews() {
      if (!gameId || loading || reviewSort === lastLoadedReviewSort) {
        return;
      }

      try {
        const reviewResponse = await getReviewsByGameId(gameId, reviewSort);

        if (mounted) {
          setReviews(reviewResponse ?? []);
          setLastLoadedReviewSort(reviewSort);
        }
      } catch {
        if (mounted) {
          setReviews([]);
        }
      }
    }

    loadReviews();

    return () => {
      mounted = false;
    };
  }, [gameId, reviewSort, loading, lastLoadedReviewSort]);

  const sortedScreenshots = useMemo(() => {
    return [...(game?.images ?? [])].sort((left, right) => left.displayOrder - right.displayOrder);
  }, [game?.images]);

  if (loading) {
    return (
      <View className="flex-1 items-center justify-center" style={{ backgroundColor: colors.background }}>
        <ActivityIndicator size="large" color={colors.accent} />
      </View>
    );
  }

  if (error || !game) {
    return (
      <View className="flex-1 items-center justify-center px-6" style={{ backgroundColor: colors.background }}>
        <Text className="text-center text-lg font-semibold" style={{ color: colors.text }}>
          {error || 'Game not found.'}
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
    <View style={{ flex: 1, backgroundColor: colors.background }}>
      <ScrollView contentContainerStyle={{ paddingBottom: 144 }}>
        <View className="px-4 pt-4">
          <Pressable onPress={() => navigation.goBack()} className="self-start rounded-full px-4 py-2" style={{ backgroundColor: colors.surface }}>
            <Text className="font-semibold" style={{ color: colors.text }}>
              Back
            </Text>
          </Pressable>
        </View>

        {game.coverArtUrl ? (
          <Image
            source={{ uri: game.coverArtUrl }}
            style={{ width: '100%', height: 320, backgroundColor: colors.surface }}
            contentFit="contain"
          />
        ) : (
          <View style={{ width: '100%', height: 320, backgroundColor: '#121A28' }} />
        )}

        {sortedScreenshots.length ? (
          <ScrollView horizontal showsHorizontalScrollIndicator={false} contentContainerStyle={{ paddingHorizontal: spacing.page, paddingVertical: 16 }}>
            {sortedScreenshots.map((image) => (
              <Image
                key={`${image.imageUrl}-${image.displayOrder}`}
                source={{ uri: image.imageUrl }}
                style={{ width: Dimensions.get('window').width * 0.7, height: 180, marginRight: 12, borderRadius: 20, backgroundColor: colors.surface }}
                contentFit="contain"
              />
            ))}
          </ScrollView>
        ) : null}

        <View className="px-4 pt-2">
          <Text className="text-3xl font-bold" style={{ color: colors.text }}>
            {game.title}
          </Text>
          <View className="mt-4 gap-2">
            <View className="flex-row flex-wrap gap-4">
              <View>
                <Text className="text-xs uppercase tracking-[1.5px]" style={{ color: colors.muted }}>
                  Developer
                </Text>
                <Text className="text-base font-medium" style={{ color: colors.text }}>
                  {game.developer}
                </Text>
              </View>
              <View>
                <Text className="text-xs uppercase tracking-[1.5px]" style={{ color: colors.muted }}>
                  Publisher
                </Text>
                <Text className="text-base font-medium" style={{ color: colors.text }}>
                  {game.publisher}
                </Text>
              </View>
              <View>
                <Text className="text-xs uppercase tracking-[1.5px]" style={{ color: colors.muted }}>
                  Release Date
                </Text>
                <Text className="text-base font-medium" style={{ color: colors.text }}>
                  {dayjs(game.releaseDate).format('DD MMM YYYY')}
                </Text>
              </View>
            </View>

            <View className="mt-2 flex-row flex-wrap gap-2">
              {(game.categories ?? []).map((category) => (
                <View key={category} className="rounded-full border px-3 py-1" style={chipContainer()}>
                  <Text className="text-xs font-medium" style={{ color: colors.text }}>
                    {category}
                  </Text>
                </View>
              ))}
            </View>

            <View className="mt-1 flex-row flex-wrap gap-2">
              {(game.platforms ?? []).map((platform) => (
                <View key={platform} className="rounded-full border px-3 py-1" style={chipContainer()}>
                  <Text className="text-xs font-medium" style={{ color: colors.text }}>
                    {platform}
                  </Text>
                </View>
              ))}
            </View>

            <View className="mt-4">
              <RatingBadge averageRating={game.averageRating} reviewCount={game.reviewCount} />
            </View>

            <View className="mt-5 rounded-3xl border p-4" style={{ backgroundColor: colors.surface, borderColor: colors.border }}>
              <Text
                className="text-base leading-7"
                style={{ color: colors.text }}
                numberOfLines={descriptionExpanded ? undefined : 4}
                onTextLayout={(event) => {
                  if (event.nativeEvent.lines.length > 4) {
                    setDescriptionCanToggle(true);
                  }
                }}>
                {game.description}
              </Text>
              {descriptionCanToggle ? (
                <Pressable onPress={() => setDescriptionExpanded((current) => !current)} className="mt-3 self-start">
                  <Text className="text-sm font-semibold" style={{ color: colors.accent }}>
                    {descriptionExpanded ? 'Show less' : 'Read more'}
                  </Text>
                </Pressable>
              ) : null}
            </View>
          </View>

          <View className="mt-6">
            <View className="mb-3">
              <Text className="text-lg font-semibold" style={{ color: colors.text }}>
                Reviews
              </Text>
              <View className="mt-3">
                <SortPicker options={reviewSortOptions} selected={reviewSort} onSelect={setReviewSort} />
              </View>
            </View>

            {reviews.map((review) => (
              <ReviewCard key={review.reviewId} review={review} />
            ))}

            {!reviews.length ? (
              <View className="rounded-3xl border px-4 py-6" style={{ backgroundColor: colors.surface, borderColor: colors.border }}>
                <Text className="text-sm" style={{ color: colors.muted }}>
                  No reviews available.
                </Text>
              </View>
            ) : null}
          </View>
        </View>
      </ScrollView>

      <View
        className="absolute bottom-0 left-0 right-0 border-t px-4 py-4"
        style={{ backgroundColor: colors.surface, borderTopColor: colors.border }}>
        <View className="flex-row items-center justify-between gap-4">
          <Text className="flex-1 text-sm font-medium" style={{ color: colors.text }}>
            Sign in to track this game and write a review
          </Text>
          <Pressable onPress={() => navigation.navigate('Login')} className="rounded-full px-4 py-3" style={{ backgroundColor: colors.accent }}>
            <Text className="font-semibold" style={{ color: '#04120B' }}>
              Sign In
            </Text>
          </Pressable>
        </View>
      </View>
    </View>
  );
}