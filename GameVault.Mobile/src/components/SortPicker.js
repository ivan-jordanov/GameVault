import { ScrollView, Pressable, Text } from 'react-native';

import { colors } from '../theme';

export default function SortPicker({ options = [], selected, onSelect }) {
  return (
    <ScrollView horizontal showsHorizontalScrollIndicator={false} contentContainerStyle={{ gap: 8 }}>
      {options.map((option) => {
        const active = option.value === selected;

        return (
          <Pressable
            key={option.value}
            onPress={() => onSelect(option.value)}
            className="rounded-full border px-4 py-2"
            style={{
              borderColor: active ? colors.accent : colors.border,
              backgroundColor: active ? colors.accentSoft : colors.surface,
            }}>
            <Text className="text-sm font-semibold" style={{ color: active ? colors.accent : colors.text }}>
              {option.label}
            </Text>
          </Pressable>
        );
      })}
    </ScrollView>
  );
}