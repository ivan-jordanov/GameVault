import { Text, View } from 'react-native';

import { colors } from '../theme';

export default function RatingBadge({ averageRating, reviewCount }) {
  const isRated = averageRating !== null && averageRating !== undefined;

  return (
    <View className="items-start">
      <View
        className="rounded-full px-3 py-1"
        style={{ backgroundColor: isRated ? colors.accentSoft : '#232A36' }}>
        <Text className="text-xs font-semibold" style={{ color: isRated ? colors.accent : colors.muted }}>
          {isRated ? `${Number(averageRating).toFixed(1)} / 10` : 'Unrated'}
        </Text>
      </View>
      <Text className="mt-1 text-[11px]" style={{ color: colors.muted }}>
        {reviewCount} reviews
      </Text>
    </View>
  );
}