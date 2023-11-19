import { HttpHandlerFn, HttpRequest } from '@angular/common/http';

export function secureApiInterceptor(
  request: HttpRequest<unknown>,
  next: HttpHandlerFn
) {
  const secureRoutes = [getApiUrl()];

  if (!secureRoutes.find((x) => request.url.startsWith(x))) {
    return next(request);
  }

  request = request.clone({
    headers: request.headers.set('X-CSRF', '1'),
  });

  return next(request);
}

export function getApiUrl() {
  const backendHost = getCurrentHost();

  return `${backendHost}/api/`;
}

function getCurrentHost() {
  const host = window.location.host;
  const url = `${window.location.protocol}//${host}`;
  return url;
}
