import { Component, OnInit } from '@angular/core';
import { ApiService } from '../services/api.service';
import { Value } from '../models/value';

@Component({
  selector: 'app-value',
  templateUrl: './value.component.html',
  styleUrls: ['./value.component.css']
})
export class ValueComponent implements OnInit {

  values: Value[] = [];

  constructor(private apiservice: ApiService<Value>) { }

  ngOnInit(): void {
    this.getValues();
  }

  private getValues(): void {
    this.apiservice.getAll(Value).subscribe(
      response => {
        this.values = response;
      },
      error => {
        console.log(error);
      });
  }

}
