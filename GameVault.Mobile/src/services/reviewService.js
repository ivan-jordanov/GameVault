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

export function getReviewsByGameId(gameId, sort) {
  return requestJson(`/reviews/game/${gameId}${createQuery({ sort })}`);
}