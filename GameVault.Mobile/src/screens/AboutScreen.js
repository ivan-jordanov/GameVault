import { useEffect, useMemo, useState } from 'react';
import { ActivityIndicator, Pressable, ScrollView, Text, useWindowDimensions, View } from 'react-native';
import RenderHtml from 'react-native-render-html';

import { getAllWebResources } from '../services/webResourceService';
import { colors, spacing } from '../theme';

const tabs = [
  { key: 'about', label: 'About', title: 'About' },
  { key: 'terms', label: 'Terms of Use', title: 'Terms of Use' },
  { key: 'community', label: 'Community Guidelines', title: 'Community Guidelines' },
];

export default function AboutScreen() {
  const { width } = useWindowDimensions();
  const [resources, setResources] = useState([]);
  const [selectedTab, setSelectedTab] = useState('about');
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');

  useEffect(() => {
    let mounted = true;

    async function loadResources() {
      try {
        setLoading(true);
        setError('');

        const response = await getAllWebResources();

        if (mounted) {
          setResources(response ?? []);
        }
      } catch (loadError) {
        if (mounted) {
          setError(loadError.message || 'Failed to load resources.');
        }
      } finally {
        if (mounted) {
          setLoading(false);
        }
      }
    }

    loadResources();

    return () => {
      mounted = false;
    };
  }, []);

  const selectedResource = useMemo(() => {
    const tab = tabs.find((item) => item.key === selectedTab);
    return resources.find((resource) => resource.title === tab?.title) ?? null;
  }, [resources, selectedTab]);

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
      </View>
    );
  }

  return (
    <ScrollView style={{ backgroundColor: colors.background }} contentContainerStyle={{ padding: spacing.page, paddingBottom: 120 }}>
      <Text className="text-3xl font-bold" style={{ color: colors.text }}>
        About
      </Text>

      <View className="mt-4 flex-row rounded-full border p-1" style={{ backgroundColor: colors.surface, borderColor: colors.border }}>
        {tabs.map((tab) => {
          const active = selectedTab === tab.key;

          return (
            <Pressable
              key={tab.key}
              onPress={() => setSelectedTab(tab.key)}
              className="flex-1 rounded-full px-3 py-3"
              style={{ backgroundColor: active ? colors.accentSoft : 'transparent' }}>
              <Text className="text-center text-xs font-semibold" style={{ color: active ? colors.accent : colors.muted }}>
                {tab.label}
              </Text>
            </Pressable>
          );
        })}
      </View>

      <View className="mt-5 rounded-3xl border p-4" style={{ backgroundColor: colors.surface, borderColor: colors.border }}>
        {selectedResource ? (
          <RenderHtml
            contentWidth={width - spacing.page * 2 - 32}
            source={{ html: selectedResource.htmlContent }}
            tagsStyles={{
              body: { color: colors.text, fontSize: 16, lineHeight: 26 },
              p: { color: colors.text, marginBottom: 14 },
              h1: { color: colors.text, fontSize: 28, marginBottom: 14 },
              h2: { color: colors.text, fontSize: 22, marginBottom: 12 },
              h3: { color: colors.text, fontSize: 18, marginBottom: 10 },
              li: { color: colors.text, marginBottom: 6 },
              a: { color: colors.accent },
            }}
          />
        ) : (
          <Text style={{ color: colors.muted }}>No content available.</Text>
        )}
      </View>
    </ScrollView>
  );
}