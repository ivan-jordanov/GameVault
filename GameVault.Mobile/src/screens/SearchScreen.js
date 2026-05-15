import { useCallback, useEffect, useMemo, useState } from 'react';
import { ActivityIndicator, Pressable, ScrollView, Text, TextInput, View } from 'react-native';

import FilterChips from '../components/FilterChips';
import GameCard from '../components/GameCard';
import { getCategories, getPlatforms, searchGames } from '../services/gameService';
import { colors, spacing } from '../theme';

export default function SearchScreen({ navigation }) {
  const [query, setQuery] = useState('');
  const [categories, setCategories] = useState([]);
  const [platforms, setPlatforms] = useState([]);
  const [selectedCategoryIds, setSelectedCategoryIds] = useState([]);
  const [selectedPlatformIds, setSelectedPlatformIds] = useState([]);
  const [results, setResults] = useState([]);
  const [loading, setLoading] = useState(true);
  const [searchLoading, setSearchLoading] = useState(false);
  const [error, setError] = useState('');
  const [reloadTick, setReloadTick] = useState(0);
  const [lastSearchSummary, setLastSearchSummary] = useState('All games');

  const runSearch = useCallback(async () => {
    const trimmedQuery = query.trim();

    try {
      setSearchLoading(true);
      setError('');

      const categoryId = selectedCategoryIds[0] ?? null;
      const platformId = selectedPlatformIds[0] ?? null;
      const searchResults = await searchGames(trimmedQuery, categoryId, platformId);

      setResults(searchResults ?? []);

      const summaryParts = [];

      if (trimmedQuery) {
        summaryParts.push(`"${trimmedQuery}"`);
      }

      if (selectedCategoryIds.length) {
        summaryParts.push(`${selectedCategoryIds.length} category`);
      }

      if (selectedPlatformIds.length) {
        summaryParts.push(`${selectedPlatformIds.length} platform`);
      }

      setLastSearchSummary(summaryParts.length ? summaryParts.join(' + ') : 'All games');
    } catch (searchError) {
      setError(searchError.message || 'Failed to search games.');
    } finally {
      setSearchLoading(false);
    }
  }, [query, selectedCategoryIds, selectedPlatformIds]);

  useEffect(() => {
    let mounted = true;

    async function loadInitialData() {
      try {
        setLoading(true);
        setError('');

        const [categoriesResponse, platformsResponse, initialResults] = await Promise.all([
          getCategories(),
          getPlatforms(),
          searchGames('', null, null),
        ]);

        if (!mounted) {
          return;
        }

        setCategories(categoriesResponse ?? []);
        setPlatforms(platformsResponse ?? []);
        setResults(initialResults ?? []);
        setLastSearchSummary('All games');
      } catch (loadError) {
        if (mounted) {
          setError(loadError.message || 'Failed to load search filters.');
        }
      } finally {
        if (mounted) {
          setLoading(false);
        }
      }
    }

    loadInitialData();

    return () => {
      mounted = false;
    };
  }, [reloadTick]);

  const resultLabel = useMemo(() => `${results.length} results found`, [results.length]);

  if (loading) {
    return (
      <View className="flex-1 items-center justify-center" style={{ backgroundColor: colors.background }}>
        <ActivityIndicator size="large" color={colors.accent} />
      </View>
    );
  }

  if (error && !results.length) {
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
      <Text className="text-3xl font-bold" style={{ color: colors.text }}>
        Search
      </Text>
      <TextInput
        value={query}
        onChangeText={setQuery}
        onSubmitEditing={runSearch}
        returnKeyType="search"
        placeholder="Search games by title"
        placeholderTextColor={colors.muted}
        className="mt-4 rounded-2xl border px-4 py-3 text-base"
        style={{ backgroundColor: colors.surface, borderColor: colors.border, color: colors.text }}
      />

      <View className="mt-5 space-y-4">
        <FilterChips
          label="Categories"
          items={categories}
          selected={selectedCategoryIds}
          onSelect={setSelectedCategoryIds}
        />
        <FilterChips
          label="Platforms"
          items={platforms}
          selected={selectedPlatformIds}
          onSelect={setSelectedPlatformIds}
        />

        <Pressable
          onPress={runSearch}
          disabled={searchLoading}
          className="mt-2 items-center rounded-2xl px-4 py-3"
          style={{
            backgroundColor: searchLoading ? colors.muted : colors.accent,
            opacity: searchLoading ? 0.8 : 1,
          }}>
          <Text className="text-base font-semibold" style={{ color: '#04120B' }}>
            {searchLoading ? 'Searching...' : 'Search'}
          </Text>
        </Pressable>
      </View>

      <View className="mt-6 flex-row items-center justify-between">
        <Text className="text-base font-semibold" style={{ color: colors.text }}>
          {resultLabel}
        </Text>
        <Text className="text-xs" style={{ color: colors.muted }}>
          {lastSearchSummary}
        </Text>
      </View>

      {!results.length && !searchLoading ? (
        <View className="mt-8 items-center justify-center rounded-3xl border px-5 py-10" style={{ backgroundColor: colors.surface, borderColor: colors.border }}>
          <Text className="text-base font-semibold" style={{ color: colors.text }}>
            No results found
          </Text>
        </View>
      ) : null}

      <View className="mt-4">
        {results.map((game) => (
          <GameCard key={game.gameId} game={game} horizontal={false} onPress={() => navigation.navigate('GameDetail', { gameId: game.gameId })} />
        ))}
      </View>
    </ScrollView>
  );
}