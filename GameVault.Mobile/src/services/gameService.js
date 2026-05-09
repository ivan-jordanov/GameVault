import { requestJson } from './apiClient';

function createQuery(params) {
  const entries = Object.entries(params).filter(([, value]) => value !== null && value !== undefined);
  if (!entries.length) {
    return '';
  }

  const searchParams = new URLSearchParams();

  entries.forEach(([key, value]) => {
    searchParams.append(key, String(value));
  });

  return `?${searchParams.toString()}`;
}

export function getGames(sort) {
  return requestJson(`/games${createQuery({ sort })}`);
}

export function getGameById(id) {
  return requestJson(`/games/${id}`);
}

export function searchGames(q, categoryId, platformId) {
  return requestJson(
    `/games/search${createQuery({
      q: q ?? '',
      categoryId,
      platformId,
    })}`,
  );
}

export function getCategories() {
  return requestJson('/categories');
}

export function getPlatforms() {
  return requestJson('/platforms');
}