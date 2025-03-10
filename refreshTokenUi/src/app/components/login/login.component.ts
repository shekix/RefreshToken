import { Component } from '@angular/core';
import { AuthServiceService } from '../../auth-service.service';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {
  username: string = '';
  password: string = '';

  constructor(private authService: AuthServiceService,private router:Router) {}

  onLogin() {
    this.authService.login(this.username, this.password).subscribe(response => {
      if (response) {
        this.authService.storeTokens(response.accessToken, response.refreshToken);
        alert('Login Successful');
        this.router.navigate(['dashboard'])
      }
    }, error => {
      console.error('Login failed', error);
      alert('Login failed');
    });
  }
}
