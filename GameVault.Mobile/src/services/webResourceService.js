import { requestJson } from './apiClient';

export function getAllWebResources() {
  return requestJson('/webresources');
}

export function getWebResourceById(id) {
  return requestJson(`/webresources/${id}`);
}