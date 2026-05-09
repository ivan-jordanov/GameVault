import { Pressable, Text, View } from 'react-native';
import { Image } from 'expo-image';

import RatingBadge from './RatingBadge';
import { colors } from '../theme';

function Placeholder({ horizontal }) {
  return (
    <View
      className="items-center justify-center"
      style={{
        width: '100%',
        height: horizontal ? 110 : 164,
        backgroundColor: '#121A28',
      }}>
      <Text className="text-xs font-semibold uppercase tracking-[1.5px]" style={{ color: colors.muted }}>
        No cover
      </Text>
    </View>
  );
}

export default function GameCard({ game, onPress, horizontal = true }) {
  const platform = game.platforms?.[0] ?? 'Unknown platform';

  if (horizontal) {
    return (
      <Pressable
        onPress={onPress}
        className="mr-3 overflow-hidden rounded-3xl border"
        style={{ width: 156, backgroundColor: colors.surface, borderColor: colors.border }}>
        {game.coverArtUrl ? (
          <Image source={{ uri: game.coverArtUrl }} style={{ width: '100%', height: 110 }} contentFit="cover" />
        ) : (
          <Placeholder horizontal />
        )}
        <View className="space-y-2 p-3">
          <Text className="text-sm font-semibold" style={{ color: colors.text }} numberOfLines={2}>
            {game.title}
          </Text>
          <Text className="text-xs" style={{ color: colors.muted }} numberOfLines={1}>
            {platform}
          </Text>
          <RatingBadge averageRating={game.averageRating} reviewCount={game.reviewCount} />
        </View>
      </Pressable>
    );
  }

  return (
    <Pressable
      onPress={onPress}
      className="mb-3 overflow-hidden rounded-3xl border"
      style={{ backgroundColor: colors.surface, borderColor: colors.border }}>
      <View className="flex-row">
        {game.coverArtUrl ? (
          <Image source={{ uri: game.coverArtUrl }} style={{ width: 108, height: 148 }} contentFit="cover" />
        ) : (
          <View style={{ width: 108, height: 148, backgroundColor: '#121A28' }} />
        )}
        <View className="flex-1 justify-between p-4">
          <View className="space-y-1">
            <Text className="text-base font-semibold" style={{ color: colors.text }} numberOfLines={2}>
              {game.title}
            </Text>
            <Text className="text-sm" style={{ color: colors.muted }} numberOfLines={1}>
              {platform}
            </Text>
            <Text className="text-xs" style={{ color: colors.muted }} numberOfLines={1}>
              {game.developer}
            </Text>
          </View>
          <RatingBadge averageRating={game.averageRating} reviewCount={game.reviewCount} />
        </View>
      </View>
    </Pressable>
  );
}