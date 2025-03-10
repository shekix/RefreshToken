import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { CookieService } from 'ngx-cookie-service';

@Injectable({
  providedIn: 'root'
})
export class AuthServiceService {

  private apiUrl = 'https://localhost:7295/api/Auth';

  constructor(private http: HttpClient, private cookieService: CookieService) {}

  login(username: string, password: string): Observable<any> {
    return this.http.post(`${this.apiUrl}/login`, { username, password });
  }

  refreshToken(accessToken: string, refreshToken: string): Observable<any> {
    return this.http.post(`${this.apiUrl}/refresh-token`, { accessToken, refreshToken });
  }

  storeTokens(accessToken: string, refreshToken: string): void {
    this.cookieService.set('accessToken', accessToken);
    this.cookieService.set('refreshToken', refreshToken);
  }

  getAccessToken(): string {
    return this.cookieService.get('accessToken');
  }

  getRefreshToken(): string {
    return this.cookieService.get('refreshToken');
  }
  
  fetchResult():Observable<any>{
    return this.http.get(`${this.apiUrl}/demo-result`);
  }
}
