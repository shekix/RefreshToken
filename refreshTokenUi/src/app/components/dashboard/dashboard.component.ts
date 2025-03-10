import { Component, OnInit } from '@angular/core';
import { AuthServiceService } from '../../auth-service.service';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.css'
})
export class DashboardComponent implements OnInit{

  constructor(private authService:AuthServiceService){}
  result:any;
  ngOnInit(): void {
    this.authService.fetchResult().subscribe(response=>{
      this.result = response.result;
    })
  }
}
