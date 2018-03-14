import { Component, EventEmitter, Input, OnChanges, OnInit, Output } from '@angular/core';

@Component({
    selector: 'pagination',
    template: `
        <nav *ngIf="totalItems > pageSize">
            <ul class="pagination">
                <li [class.disabled]="currentPage == 1">
                    <a (click)="previous()" aria-label="Previous">
                    <span aria-hidden="true">&laquo;</span>
                    </a>
                </li>
                <li [class.active]="currentPage == page" *ngFor="let page of pages" (click)="changePage(page)">
                    <a>{{ page }}</a>
                </li>
                <li [class.disabled]="currentPage == pages.length">
                    <a (click)="next()" aria-label="Next">
                    <span aria-hidden="true">&raquo;</span>
                    </a>
                </li>
            </ul>
        </nav>
    `,
    styles: [``]
})
export class PaginationComponent implements OnChanges {

    @Input('total-items') totalItems: number;
    @Input('page-size') pageSize = 10;
    @Output('page-changed') pageChanged = new EventEmitter;
    pages: any[];
    currentPage = 1;


    constructor() { }

    ngOnInit() { }

    ngOnChanges() {
        this.currentPage = 1;

        let pagesCount = Math.ceil(this.totalItems / this.pageSize);
        this.pages = [];
        for (var i = 1; i <= pagesCount; i++) {
            this.pages.push(i);
        }
        console.log(this);
    }

    changePage(page: number) {
        this.currentPage = page;
        this.pageChanged.emit(page);
    }

    previous() {
        if (this.currentPage == 1)
            return;

        this.currentPage--;
        this.pageChanged.emit(this.currentPage);
    }

    next() {
        if (this.currentPage == this.pages.length)
            return;

        this.currentPage++;
        this.pageChanged.emit(this.currentPage);
    }
}