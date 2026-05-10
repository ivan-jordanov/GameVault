import { API_BASE_URL } from '../config';

const REQUEST_TIMEOUT_MS = 10000;

async function parseResponse(response) {
  const text = await response.text();

  if (!text) {
    return null;
  }

  try {
    return JSON.parse(text);
  } catch {
    return text;
  }
}

export async function requestJson(path, options = {}) {
  const controller = new AbortController();
  const timeoutId = setTimeout(() => controller.abort(), REQUEST_TIMEOUT_MS);

  let response;

  try {
    response = await fetch(`${API_BASE_URL}${path}`, {
      ...options,
      headers: {
        Accept: 'application/json',
        ...(options.headers ?? {}),
      },
      signal: options.signal ?? controller.signal,
    });
  } catch (error) {
    if (error?.name === 'AbortError') {
      throw new Error(
        `Request timed out after ${REQUEST_TIMEOUT_MS / 1000}s. Check API URL (${API_BASE_URL}) and backend availability.`,
      );
    }

    const message = error?.message || 'Network request failed.';
    throw new Error(`Network request failed for ${API_BASE_URL}${path}. ${message}`);
  } finally {
    clearTimeout(timeoutId);
  }

  const body = await parseResponse(response);

  if (!response.ok) {
    const message =
      body && typeof body === 'object'
        ? body.message || body.title || JSON.stringify(body)
        : typeof body === 'string'
          ? body
          : `Request failed with status ${response.status}`;

    throw new Error(message);
  }

  return body;
}