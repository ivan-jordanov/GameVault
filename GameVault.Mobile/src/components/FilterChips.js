import { ScrollView, Pressable, Text, View } from 'react-native';

import { colors } from '../theme';

function getItemId(item) {
  return item.id ?? item.categoryId ?? item.platformId;
}

export default function FilterChips({ items = [], selected, onSelect, label }) {
  return (
    <View className="space-y-2">
      <Text className="text-sm font-semibold uppercase tracking-[1.5px]" style={{ color: colors.muted }}>
        {label}
      </Text>
      <ScrollView horizontal showsHorizontalScrollIndicator={false} contentContainerStyle={{ gap: 8 }}>
        {items.map((item) => {
          const itemId = getItemId(item);
          const active = selected === itemId;

          return (
            <Pressable
              key={`${label}-${itemId}`}
              onPress={() => onSelect(active ? null : itemId)}
              className="rounded-full border px-4 py-2"
              style={{
                borderColor: active ? colors.accent : colors.border,
                backgroundColor: active ? colors.accentSoft : colors.surface,
              }}>
              <Text className="text-sm font-medium" style={{ color: active ? colors.accent : colors.text }}>
                {item.name}
              </Text>
            </Pressable>
          );
        })}
      </ScrollView>
    </View>
  );
}