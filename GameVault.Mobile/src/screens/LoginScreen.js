import { Pressable, Text, View } from 'react-native';

import { colors } from '../theme';

export default function LoginScreen({ navigation }) {
  return (
    <View className="flex-1 items-center justify-center px-6" style={{ backgroundColor: colors.background }}>
      <Pressable onPress={() => navigation.goBack()} className="absolute left-4 top-16 rounded-full px-4 py-2" style={{ backgroundColor: colors.surface }}>
        <Text className="font-semibold" style={{ color: colors.text }}>
          Close
        </Text>
      </Pressable>

      <Text className="text-4xl font-bold" style={{ color: colors.text }}>
        GameVault
      </Text>
      <Text className="mt-4 text-center text-base" style={{ color: colors.muted }}>
        Login and Registration coming soon
      </Text>
    </View>
  );
}