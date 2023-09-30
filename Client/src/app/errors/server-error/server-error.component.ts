import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-server-error',
  templateUrl: './server-error.component.html',
  styleUrls: ['./server-error.component.css']
})
export class ServerErrorComponent implements OnInit {
  error: any;
  errorMessage: any;

  constructor(private router: Router) { 
    debugger
    const navigation = this.router.getCurrentNavigation();
    this.error = navigation?.extras?.state?.error;
    this.errorMessage = navigation?.extras?.state?.message;
  }

  ngOnInit(): void {
  }

}
