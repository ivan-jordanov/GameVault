import { Image } from 'expo-image';
import dayjs from 'dayjs';
import { useState } from 'react';
import { Pressable, Text, View } from 'react-native';

import { colors } from '../theme';

function ratingStyle(rating) {
  if (rating >= 8) {
    return { backgroundColor: '#113725', color: colors.accent };
  }

  if (rating >= 5) {
    return { backgroundColor: '#3E3314', color: colors.warning };
  }

  return { backgroundColor: '#3E1820', color: colors.danger };
}

export default function ReviewCard({ review, showGameTitle = false }) {
  const [expanded, setExpanded] = useState(false);
  const badge = ratingStyle(review.rating);
  const shouldToggle = review.reviewText && review.reviewText.length > 140;

  return (
    <View className="mb-3 rounded-3xl border p-4" style={{ backgroundColor: colors.surface, borderColor: colors.border }}>
      <View className="flex-row items-center justify-between">
        <View className="flex-row items-center gap-3">
          {review.profileImageUrl ? (
            <Image source={{ uri: review.profileImageUrl }} style={{ width: 40, height: 40, borderRadius: 20 }} />
          ) : (
            <View className="h-10 w-10 items-center justify-center rounded-full" style={{ backgroundColor: '#121A28' }}>
              <Text className="text-sm font-semibold" style={{ color: colors.muted }}>
                {review.username?.[0]?.toUpperCase() ?? '?'}
              </Text>
            </View>
          )}
          <Text className="text-sm font-semibold" style={{ color: colors.text }}>
            {review.username}
          </Text>
        </View>
        <Text className="text-xs" style={{ color: colors.muted }}>
          {dayjs(review.createdAt).format('DD MMM YYYY')}
        </Text>
      </View>

      <View className="mt-3 self-start rounded-full px-2 py-1" style={{ backgroundColor: badge.backgroundColor }}>
        <Text className="text-xs font-semibold" style={{ color: badge.color }}>
          {review.rating}/10
        </Text>
      </View>

      {showGameTitle ? (
        <Text className="mt-3 text-sm font-semibold" style={{ color: colors.accent }} numberOfLines={1}>
          {review.gameTitle}
        </Text>
      ) : null}

      <Text className="mt-2 text-sm leading-6" style={{ color: colors.text }} numberOfLines={expanded ? undefined : 3}>
        {review.reviewText}
      </Text>

      {shouldToggle ? (
        <Pressable onPress={() => setExpanded((current) => !current)} className="mt-2 self-start">
          <Text className="text-sm font-semibold" style={{ color: colors.accent }}>
            {expanded ? 'Show less' : 'Read more'}
          </Text>
        </Pressable>
      ) : null}
    </View>
  );
}