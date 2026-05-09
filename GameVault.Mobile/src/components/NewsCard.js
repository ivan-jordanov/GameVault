import { Text, View } from 'react-native';
import dayjs from 'dayjs';

import { colors } from '../theme';

export default function NewsCard({ news }) {
  return (
    <View
      className="mr-3 w-[260px] rounded-3xl border p-4"
      style={{ backgroundColor: colors.surface, borderColor: colors.border }}>
      <Text className="text-base font-semibold" style={{ color: colors.text }} numberOfLines={2}>
        {news.title}
      </Text>
      <Text className="mt-2 text-sm leading-5" style={{ color: colors.muted }} numberOfLines={2}>
        {news.content}
      </Text>
      <Text className="mt-3 text-xs font-medium" style={{ color: colors.accent }}>
        {dayjs(news.publishDate).format('DD MMM YYYY')}
      </Text>
    </View>
  );
}