import { useEffect, useMemo, useState } from 'react';
import { ActivityIndicator, Pressable, ScrollView, Text, TextInput, View } from 'react-native';

import FilterChips from '../components/FilterChips';
import GameCard from '../components/GameCard';
import { getCategories, getPlatforms, searchGames } from '../services/gameService';
import { colors, spacing } from '../theme';

export default function SearchScreen({ navigation }) {
  const [query, setQuery] = useState('');
  const [debouncedQuery, setDebouncedQuery] = useState('');
  const [categories, setCategories] = useState([]);
  const [platforms, setPlatforms] = useState([]);
  const [selectedCategoryId, setSelectedCategoryId] = useState(null);
  const [selectedPlatformId, setSelectedPlatformId] = useState(null);
  const [results, setResults] = useState([]);
  const [loading, setLoading] = useState(true);
  const [searchLoading, setSearchLoading] = useState(false);
  const [error, setError] = useState('');
  const [reloadTick, setReloadTick] = useState(0);
  const [lastLoadedSearchKey, setLastLoadedSearchKey] = useState('');

  useEffect(() => {
    const timer = setTimeout(() => {
      setDebouncedQuery(query.trim());
    }, 300);

    return () => clearTimeout(timer);
  }, [query]);

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
        setLastLoadedSearchKey('||');
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

  useEffect(() => {
    let mounted = true;
    const searchKey = `${debouncedQuery}|${selectedCategoryId ?? ''}|${selectedPlatformId ?? ''}`;

    if (searchKey === lastLoadedSearchKey) {
      return () => {
        mounted = false;
      };
    }

    async function runSearch() {
      try {
        setSearchLoading(true);
        setError('');

        const searchResults = await searchGames(debouncedQuery, selectedCategoryId, selectedPlatformId);

        if (mounted) {
          setResults(searchResults ?? []);
          setLastLoadedSearchKey(searchKey);
        }
      } catch (searchError) {
        if (mounted) {
          setError(searchError.message || 'Failed to search games.');
        }
      } finally {
        if (mounted) {
          setSearchLoading(false);
        }
      }
    }

    if (!loading) {
      runSearch();
    }

    return () => {
      mounted = false;
    };
  }, [debouncedQuery, selectedCategoryId, selectedPlatformId, lastLoadedSearchKey]);

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
        placeholder="Search games by title"
        placeholderTextColor={colors.muted}
        className="mt-4 rounded-2xl border px-4 py-3 text-base"
        style={{ backgroundColor: colors.surface, borderColor: colors.border, color: colors.text }}
      />

      <View className="mt-5 space-y-4">
        <FilterChips
          label="Categories"
          items={categories}
          selected={selectedCategoryId}
          onSelect={setSelectedCategoryId}
        />
        <FilterChips
          label="Platforms"
          items={platforms}
          selected={selectedPlatformId}
          onSelect={setSelectedPlatformId}
        />
      </View>

      <View className="mt-6 flex-row items-center justify-between">
        <Text className="text-base font-semibold" style={{ color: colors.text }}>
          {resultLabel}
        </Text>
        {searchLoading ? <ActivityIndicator color={colors.accent} /> : null}
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