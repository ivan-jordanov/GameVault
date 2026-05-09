import { API_BASE_URL } from '../config';

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
  const response = await fetch(`${API_BASE_URL}${path}`, {
    headers: {
      Accept: 'application/json',
      ...(options.headers ?? {}),
    },
    ...options,
  });

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