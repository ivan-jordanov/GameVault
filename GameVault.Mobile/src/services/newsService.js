import { requestJson } from './apiClient';

export function getNews() {
  return requestJson('/news');
}